﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Tests.Dynamic.DynamicObjectMapper
{
    using Aqua.Dynamic;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class When_mapping_dynamic_objects_to_list_of_known_types
    {
        class IsKnownTypeProvider : IIsKnownTypeProvider
        {
            public bool IsKnownType(Type type) => type == typeof(CustomReferenceType);
        }

        class CustomReferenceType
        {
            public int Int32Property { get; set; }
            public string StringProperty { get; set; }
        }

        DynamicObject[] dynamicObjects;
        IEnumerable<CustomReferenceType> recreatedObjectLists;

        public When_mapping_dynamic_objects_to_list_of_known_types()
        {
            dynamicObjects = new[]
            { 
                new DynamicObject(typeof(CustomReferenceType))
                {
                    { "", new CustomReferenceType { Int32Property = 1, StringProperty="One" } }
                },
                new DynamicObject(typeof(CustomReferenceType))
                {
                    { "", new CustomReferenceType { Int32Property = 2, StringProperty="Two" } }
                },
            };

            var mapper = new DynamicObjectMapper(isKnownTypeProvider: new IsKnownTypeProvider());

            recreatedObjectLists = mapper.Map<CustomReferenceType>(dynamicObjects);
        }

        [Fact]
        public void Objects_count_should_be_two()
        {
            recreatedObjectLists.Count().ShouldBe(2);
        }

        [Fact]
        public void Objects_should_be_source_objects()
        {
            for (int i = 0; i < dynamicObjects.Length; i++)
            {
                var sourceObject = dynamicObjects.ElementAt(i).Values.Single();

                var recreatedObject = recreatedObjectLists.ElementAt(i);

                recreatedObject.ShouldBeSameAs(sourceObject);
            }
        }
    }
}
