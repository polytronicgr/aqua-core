﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Tests.Serialization.TypeSystem.TypeInfo
{
    partial class When_using_typeinfo_with_circular_reference
    {
        public class BinaryFormatter : When_using_typeinfo_with_circular_reference
        {
            public BinaryFormatter()
                : base(BinarySerializationHelper.Serialize)
            {
            }
        }
    }
}
