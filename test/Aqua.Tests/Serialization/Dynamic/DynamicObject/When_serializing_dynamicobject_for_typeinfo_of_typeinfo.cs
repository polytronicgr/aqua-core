﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Tests.Serialization.DynamicObject
{
    using Aqua.Dynamic;
    using Aqua.TypeSystem;
    using Shouldly;
    using System;
    using Xunit;

    public abstract class When_serializing_dynamicobject_for_typeinfo_of_typeinfo
    {
#pragma warning disable SA1128 // Put constructor initializers on their own line
#pragma warning disable SA1502 // Element should not be on a single line

        public class DataContractSerializer : When_serializing_dynamicobject_for_typeinfo_of_typeinfo
        {
            public DataContractSerializer() : base(DataContractSerializationHelper.Serialize) { }
        }

        public class JsonSerializer : When_serializing_dynamicobject_for_typeinfo_of_typeinfo
        {
            public JsonSerializer() : base(JsonSerializationHelper.Serialize) { }
        }

#pragma warning disable SA1512 // Single-line comments should not be followed by blank line

        // XML serialization doesn't support circular references

#pragma warning restore SA1512 // Single-line comments should not be followed by blank line

        public class BinaryFormatter : When_serializing_dynamicobject_for_typeinfo_of_typeinfo
        {
            public BinaryFormatter() : base(BinarySerializationHelper.Serialize) { }
        }

#if NET
        public class NetDataContractSerializer : When_serializing_dynamicobject_for_typeinfo_of_typeinfo
        {
            public NetDataContractSerializer() : base(NetDataContractSerializationHelper.Serialize) { }
        }
#endif

#pragma warning restore SA1502 // Element should not be on a single line
#pragma warning restore SA1128 // Put constructor initializers on their own line

        private readonly DynamicObject dynamicObject;
        private readonly DynamicObject serializedDynamicObject;

        protected When_serializing_dynamicobject_for_typeinfo_of_typeinfo(Func<DynamicObject, DynamicObject> serialize)
        {
            dynamicObject = DynamicObject.Create(new TypeInfo(typeof(TypeInfo), true));

            serializedDynamicObject = serialize(dynamicObject);
        }

        [Fact]
        public void Serialization_should_return_new_instance()
        {
            serializedDynamicObject.ShouldNotBeSameAs(dynamicObject);
        }
    }
}
