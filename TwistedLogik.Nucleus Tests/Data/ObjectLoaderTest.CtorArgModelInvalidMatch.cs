﻿using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test failure states for constructor arguments.
    /// </summary>
    public class ObjectLoader_CtorArgModelInvalidMatch : DataObject
    {
        public ObjectLoader_CtorArgModelInvalidMatch(Int32 x, Int32 y, Int32 z)
            : base(Guid.Empty)
        {

        }
    }
}