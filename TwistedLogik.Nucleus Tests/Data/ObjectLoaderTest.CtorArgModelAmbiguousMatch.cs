﻿using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test failure states for constructor arguments.
    /// </summary>
    public class ObjectLoader_CtorArgModelAmbiguousMatch : DataObject
    {
        public ObjectLoader_CtorArgModelAmbiguousMatch(Guid globalID)
            : base(globalID)
        {

        }
    }
}