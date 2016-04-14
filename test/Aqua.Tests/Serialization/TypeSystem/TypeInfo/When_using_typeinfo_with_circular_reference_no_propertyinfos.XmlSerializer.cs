﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Tests.Serialization.TypeSystem.TypeInfo
{
    partial class When_using_typeinfo_with_circular_reference_no_propertyinfos
    {
        public class XmlSerializer : When_using_typeinfo_with_circular_reference_no_propertyinfos
        {
            public XmlSerializer()
                : base(XmlSerializationHelper.Serialize)
            {
            }
        }
    }
}
