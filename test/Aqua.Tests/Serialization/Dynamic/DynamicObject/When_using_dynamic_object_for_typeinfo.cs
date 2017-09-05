﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Tests.Serialization.Dynamic.DynamicObject
{
    using Aqua.Dynamic;
    using Aqua.TypeSystem;
    using Shouldly;
    using System;
    using Xunit;

    public abstract class When_using_dynamic_object_for_typeinfo
    {
        public class DataContractSerializer : When_using_dynamic_object_for_typeinfo
        {
            public DataContractSerializer() : base(DataContractSerializationHelper.Serialize) { }
        }

        public class JsonSerializer : When_using_dynamic_object_for_typeinfo
        {
            public JsonSerializer() : base(JsonSerializationHelper.Serialize) { }
        }

        public class XmlSerializer : When_using_dynamic_object_for_typeinfo
        {
            public XmlSerializer() : base(XmlSerializationHelper.Serialize) { }
        }

#if NET
        public class BinaryFormatter : When_using_dynamic_object_for_typeinfo
        {
            public BinaryFormatter() : base(BinarySerializationHelper.Serialize) { }
        }
#endif

#if NET && !NETCOREAPP2
        public class NetDataContractSerializer : When_using_dynamic_object_for_typeinfo
        {
            public NetDataContractSerializer() : base(NetDataContractSerializationHelper.Serialize) { }
        }
#endif

        Func<DynamicObject, DynamicObject> _serialize;

        protected When_using_dynamic_object_for_typeinfo(Func<DynamicObject, DynamicObject> serialize)
        {
            _serialize = serialize;
        }

        [Theory]
        [MemberData(nameof(TestData.Types), MemberType = typeof(TestData))]
        public void Should_map_type_to_dynamic_object_and_back(Type type)
        {
            var dynamicObject = new DynamicObjectMapper().MapObject(type);
            var serialized = _serialize(dynamicObject);
            var resurectedType = (Type)new DynamicObjectMapper().Map(serialized);

            dynamicObject.Type.Type.ShouldBe(typeof(Type));
            dynamicObject[nameof(TypeInfo.Name)].ShouldBe(type.Name);
            dynamicObject[nameof(TypeInfo.Namespace)].ShouldBe(type.Namespace);

            resurectedType.ShouldBe(type);
        }
    }
}