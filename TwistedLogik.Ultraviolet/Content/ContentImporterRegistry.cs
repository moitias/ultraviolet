﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an Ultraviolet context's registry of content importers.
    /// </summary>
    public sealed partial class ContentImporterRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentImporterRegistry"/> class.
        /// </summary>
        internal ContentImporterRegistry()
        {

        }

        /// <summary>
        /// Registers the importers in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly that contains the importers to register.</param>
        public void RegisterAssembly(Assembly asm)
        {
            Contract.Require(asm, "asm");

            var importers = from type in asm.GetTypes()
                            let attrs = type.GetCustomAttributes(typeof(ContentImporterAttribute), false).Cast<ContentImporterAttribute>()
                            where attrs != null && attrs.Count() > 0
                            select new { Type = type, Attributes = attrs };

            foreach (var importer in importers)
            {
                var baseImporterType = GetBaseContentImporterType(importer.Type);
                if (baseImporterType == null)
                    throw new InvalidOperationException(UltravioletStrings.ImporterInvalidBaseClass.Format(importer.Type.FullName));

                var instance = CreateImporterInstance(importer.Type);
                foreach (var attr in importer.Attributes)
                {
                    if (registeredImporters.ContainsKey(attr.Extension))
                    {
                        throw new InvalidOperationException(
                            UltravioletStrings.ImporterAlreadyRegistered.Format(importer.Type.FullName, attr.Extension));
                    }
                    registeredImporters[attr.Extension] = new RegistryEntry(instance);
                }
            }
        }
        
        /// <summary>
        /// Finds the appropriate importer for the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension for which to find a content importer.</param>
        /// <returns>The content importer for the specified file extension, or <c>null</c> if no appropriate importer could be found.</returns>
        public IContentImporter FindImporter(String extension)
        {
            Type outputType;
            return FindImporter(extension, out outputType);
        }

        /// <summary>
        /// Finds the appropriate importer for the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension for which to find a content importer.</param>
        /// <param name="outputType">The importer's output type.</param>
        /// <returns>The content importer for the specified file extension, or <c>null</c> if no appropriate importer could be found.</returns>
        public IContentImporter FindImporter(String extension, out Type outputType)
        {
            Contract.RequireNotEmpty(extension, "extension");

            RegistryEntry entry;
            if (registeredImporters.TryGetValue(extension, out entry))
            {
                outputType = entry.OutputType;
            }
            else
            {
                outputType  =null;
            }
            return entry.Importer;
        }

        /// <summary>
        /// Registers a content importer for the specified file extension.
        /// </summary>
        /// <typeparam name="T">The type of content importer to register.</typeparam>
        /// <param name="extension">The file extension for which to register the importer.</param>
        public void RegisterImporter<T>(String extension) where T : IContentImporter
        {
            Contract.RequireNotEmpty(extension, "extension");

            var baseImporterType = GetBaseContentImporterType(typeof(T));
            if (baseImporterType == null)
                throw new InvalidOperationException(UltravioletStrings.ImporterInvalidBaseClass.Format(typeof(T).FullName));

            if (registeredImporters.ContainsKey(extension))
            {
                throw new InvalidOperationException(
                    UltravioletStrings.ImporterAlreadyRegistered.Format(typeof(T).FullName, extension));
            }
            registeredImporters[extension] = new RegistryEntry(CreateImporterInstance(typeof(T)));
        }

        /// <summary>
        /// Unregisters a content importer from the specified file extension.
        /// </summary>
        /// <typeparam name="T">The type of content importer to unregister.</typeparam>
        /// <param name="extension">The file extension from which to unregister the importer.</param>
        public void UnregisterImporter<T>(String extension) where T : IContentImporter
        {
            Contract.RequireNotEmpty(extension, "extension");

            registeredImporters.Remove(extension);
        }

        /// <summary>
        /// Gets the ContentImporter type from which the specified type is derived.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns>The ContentImporter type from which the specified type is derived.</returns>
        internal static Type GetBaseContentImporterType(Type type)
        {
            var current = type;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(ContentImporter<>))
                {
                    return current;
                }
                current = current.BaseType;
            }
            return null;
        }

        /// <summary>
        /// Creates an instance of the specified importer type.
        /// </summary>
        /// <param name="type">The type of importer to instantiate.</param>
        /// <returns>The importer instance that was created.</returns>
        private IContentImporter CreateImporterInstance(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new InvalidOperationException(UltravioletStrings.ImporterRequiresCtor.Format(type.FullName));
            }
            return (IContentImporter)ctor.Invoke(null);
        }

        // The content importer registry.
        private readonly Dictionary<String, RegistryEntry> registeredImporters = 
            new Dictionary<String, RegistryEntry>();
    }
}
