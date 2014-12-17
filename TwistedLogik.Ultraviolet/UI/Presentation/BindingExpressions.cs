﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a method which compares two data bound values for equality.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="value1">The first value to compare.</param>
    /// <param name="value2">The second value to compare.</param>
    /// <returns><c>true</c> if the specified values are equal; otherwise, <c>false</c>.</returns>
    internal delegate Boolean DataBindingComparer<T>(T value1, T value2);

    /// <summary>
    /// Represents a method which is used to retrieve the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <returns>The current value of the bound property.</returns>
    internal delegate T DataBindingGetter<T>(Object model);

    /// <summary>
    /// Represents a method which is used to set the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <param name="value">The value to set on the bound property.</param>
    internal delegate void DataBindingSetter<T>(Object model, T value);

    /// <summary>
    /// Contains methods for creating and manipulating binding expressions.
    /// </summary>
    internal partial class BindingExpressions
    {
        /// <summary>
        /// Initializes the <see cref="BindingExpressions"/> class.
        /// </summary>
        static BindingExpressions()
        {
            miReferenceEquals = typeof(Object).GetMethod("ReferenceEquals", new[] { typeof(Object), typeof(Object) });
            miObjectEquals    = typeof(Object).GetMethod("Equals", new[] { typeof(Object) });
            miNullableEquals  = typeof(Nullable).GetMethods().Where(x => x.Name == "Equals" && x.IsGenericMethod).Single();
        }

        /// <summary>
        /// Gets a value indicating whether the specified string represents a binding expression.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns><c>true</c> if the specified string represents a binding expression; otherwise, <c>false</c>.</returns>
        public static Boolean IsBindingExpression(String expression, Boolean braces = true)
        {
            if (String.IsNullOrEmpty(expression))
                return false;

            return !braces || expression.StartsWith("{{") && expression.EndsWith("}}");
        }

        /// <summary>
        /// Parses the specified binding expression into its constituent components.
        /// </summary>
        /// <param name="expression">The binding expression to parse.</param>
        /// <param name="braces">A value indicating whether the expression's containing braces are included.</param>
        /// <returns>The specified binding expression's constituent components.</returns>
        public static IEnumerable<String> ParseBindingExpression(String expression, Boolean braces = true)
        {
            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(UltravioletStrings.InvalidBindingExpression.Format(expression));

            var path       = GetBindingMemberPathPartInternal(expression, braces);
            var components = path.Split('.');

            return components;
        }

        /// <summary>
        /// Combines two binding expressions into a single binding expression.
        /// </summary>
        /// <param name="expression1">The first binding expression.</param>
        /// <param name="expression2">The second binding expression.</param>
        /// <param name="braces">A value indicating whether the expressions' containing braces are included.</param>
        /// <returns>A binding expression that represents the combination of the specified binding expressions.</returns>
        public static String Combine(String expression1, String expression2, Boolean braces = true)
        {
            Contract.Ensure<ArgumentException>(String.IsNullOrEmpty(expression1) || IsBindingExpression(expression1, braces), "expression1");
            Contract.Ensure<ArgumentException>(String.IsNullOrEmpty(expression2) || IsBindingExpression(expression2, braces), "expression2");

            if (String.IsNullOrEmpty(expression1))
                return expression2;
            if (String.IsNullOrEmpty(expression2))
                return expression1;

            var fmt1 = GetBindingFormatStringPartInternal(expression1, braces);
            var fmt2 = GetBindingFormatStringPartInternal(expression2, braces);
            
            var path1 = GetBindingMemberPathPartInternal(expression1, braces);
            var path2 = GetBindingMemberPathPartInternal(expression2, braces);

            var combinedPath = path1 + "." + path2;
            var combinedFmt  = fmt2 ?? fmt1;
            var combinedExp  = String.Format((combinedFmt == null) ? "{0}" : "{0} | {1}", combinedPath, combinedFmt);

            return braces ? "{{" + combinedExp + "}}" : combinedExp;
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the member navigation path.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the member navigation path, or <c>null</c> if no such part exists.</returns>
        public static String GetBindingMemberPathPart(String expression, Boolean braces = true)
        {
            Contract.RequireNotEmpty(expression, "expression");

            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(UltravioletStrings.InvalidBindingExpression.Format(expression));

            return GetBindingMemberPathPartInternal(expression, braces);
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the format string.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the format string, or <c>null</c> if no such part exists.</returns>
        public static String GetBindingFormatStringPart(String expression, Boolean braces = true)
        {
            Contract.RequireNotEmpty(expression, "expression");

            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(UltravioletStrings.InvalidBindingExpression.Format(expression));

            return GetBindingFormatStringPartInternal(expression, braces);
        }

        /// <summary>
        /// Gets the type of the specified binding expression.
        /// </summary>
        /// <param name="viewModelType">The type of view model to evaluate.</param>
        /// <param name="expression">The binding expression to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The type of the binding expression.</returns>
        public static Type GetExpressionType(Type viewModelType, String expression, Boolean braces = true)
        {
            var components = ParseBindingExpression(expression);
            var currentType = viewModelType;
            foreach (var component in components)
            {
                var members = currentType.GetMember(component);
                if (members == null || !members.Any())
                    return null;

                var member = members.Where(x => 
                    x.MemberType == MemberTypes.Property ||
                    x.MemberType == MemberTypes.Field).FirstOrDefault() ?? members.First();

                currentType = GetMemberType(member);
            }
            return currentType;
        }

        /// <summary>
        /// Creates a bound event delegate.
        /// </summary>
        /// <param name="uiElement">The element to which an event is being bound.</param>
        /// <param name="viewModelType">The type of view model to which the event is being bound.</param>
        /// <param name="delegateType">The type of delegate that handles the event being bound.</param>
        /// <param name="expression">The expression which represents the path to the event handler.</param>
        /// <returns>The bound event delegate that was created.</returns>
        public static Delegate CreateBoundEventDelegate(UIElement uiElement, Type viewModelType, Type delegateType, String expression)
        {
            Contract.Require(uiElement, "element");
            Contract.Require(viewModelType, "viewModelType");
            Contract.Require(delegateType, "delegateType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new BoundEventBuilder(uiElement, delegateType, viewModelType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Creates a getter for the specified binding expression.
        /// </summary>
        /// <param name="boundType">The type of value to which the expression is being bound.</param>
        /// <param name="viewModelType">The type of view model to which the expression is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="Delegate"/> that represents the specified model and expression.</returns>
        public static Delegate CreateBindingGetter(Type boundType, Type viewModelType, String expression)
        {
            Contract.Require(boundType, "boundType");
            Contract.Require(viewModelType, "viewModelType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new DataBindingGetterBuilder(boundType, viewModelType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Creates a setter for the specified binding expression.
        /// </summary>
        /// <param name="boundType">The type of value to which the expression is being bound.</param>
        /// <param name="viewModelType">The type of view model to which the expression is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="Delegate"/> that represents the specified model and expression.</returns>
        public static Delegate CreateBindingSetter(Type boundType, Type viewModelType, String expression)
        {
            Contract.Require(boundType, "boundType");
            Contract.Require(viewModelType, "viewModelType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new DataBindingSetterBuilder(boundType, viewModelType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Gets the binding comparison function for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a binding comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        public static Delegate GetComparisonFunction(Type type)
        {
            lock (comparerRegistry)
            {
                var typeComparer = default(Delegate);

                if (!comparerRegistry.TryGetValue(type, out typeComparer))
                {
                    if (type.IsClass)
                    {
                        typeComparer = GetReferenceComparisonFunction(type);
                    }
                    else if (type.GetInterfaces().Where(x => x == typeof(IEquatable<>).MakeGenericType(type)).Any())
                    {
                        typeComparer = GetIEquatableComparisonFunction(type);
                    }
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeComparer = GetNullableComparisonFunction(type);
                    }
                    else if (type.IsEnum)
                    {
                        typeComparer = GetEnumComparisonFunction(type);
                    }
                    else
                    {
                        typeComparer = GetFallbackComparisonFunction(type);
                    }
                    comparerRegistry[type] = typeComparer;
                }

                return typeComparer;
            }
        }

        /// <summary>
        /// Gets the type of the specified member.
        /// </summary>
        /// <param name="member">The member to evaluate.</param>
        /// <returns>The type of the specified member.</returns>
        private static Type GetMemberType(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a comparison function for value types which implement <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetReferenceComparisonFunction(Type type)
        {
            var param1 = Expression.Parameter(type, "o1");
            var param2 = Expression.Parameter(type, "o2");

            var delegateBody = Expression.Call(miReferenceEquals, param1, param2);
            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, delegateBody, param1, param2).Compile();
        }

        /// <summary>
        /// Gets a comparison function for value types which implement <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetIEquatableComparisonFunction(Type type)
        {
            var equalsMethod = type.GetMethod("Equals", new[] { type });

            var arg1 = Expression.Parameter(type, "o1");
            var arg2 = Expression.Parameter(type, "o2");

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(arg1, equalsMethod, arg2), arg1, arg2).Compile();
        }

        /// <summary>
        /// Gets a comparison function for nullable value types.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetNullableComparisonFunction(Type type)
        {
            var nullableType = type.GetGenericArguments()[0];
            var nullableEqualsMethod = miNullableEquals.MakeGenericMethod(nullableType);

            var arg1 = Expression.Parameter(type, "o1");
            var arg2 = Expression.Parameter(type, "o2");

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(nullableEqualsMethod, arg1, arg2), arg1, arg2).Compile();
        }

        /// <summary>
        /// Gets a comparison function for enumeration types.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetEnumComparisonFunction(Type type)
        {
            var param1 = Expression.Parameter(type, "o1");
            var param2 = Expression.Parameter(type, "o2");

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Equal(param1, param2), param1, param2).Compile();
        }

        /// <summary>
        /// Gets a fallback comparison function for types which don't fit any optimizable category.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetFallbackComparisonFunction(Type type)
        {
            var param1 = Expression.Parameter(type, "o1");
            var param2 = Expression.Parameter(type, "o2");
            var arg1   = param1;
            var arg2   = Expression.Convert(param2, typeof(Object));

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(arg1, miObjectEquals, arg2), param1, param2).Compile();
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the member navigation path.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the member navigation path, or <c>null</c> if no such part exists.</returns>
        private static String GetBindingMemberPathPartInternal(String expression, Boolean braces = true)
        {
            var ixDelimiter = expression.IndexOf('|');
            if (ixDelimiter >= 0)
            {
                var offset = braces ? 2 : 0;
                return expression.Substring(offset, ixDelimiter - offset).Trim();
            }
            return (braces ? expression.Substring(2, expression.Length - 4) : expression).Trim();
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the format string.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the format string, or <c>null</c> if no such part exists.</returns>
        private static String GetBindingFormatStringPartInternal(String expression, Boolean braces = true)
        {
            var ixDelimiter = expression.IndexOf('|');
            if (ixDelimiter >= 0)
            {
                var offset = braces ? 2 : 0;
                var length = (expression.Length - (braces ? 2 : 0)) - (ixDelimiter + 1);
                return expression.Substring(ixDelimiter + 1, length).Trim();
            }
            return null;
        }

        // Reflection information for commonly-used methods.
        private static readonly MethodInfo miReferenceEquals;
        private static readonly MethodInfo miObjectEquals;
        private static readonly MethodInfo miNullableEquals;

        // Comparison functions for various types.
        private static readonly Dictionary<Type, Delegate> comparerRegistry = new Dictionary<Type, Delegate>();
    }
}
