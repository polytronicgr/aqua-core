﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

#if CORECLR || WINRT

namespace Aqua.TypeSystem.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MemberTypes = Aqua.TypeSystem.MemberTypes;

    partial class TypeExtensions
    {
        public static Type GetUnderlyingSystemType(this Type type)
        {
            // UnderlyingSystemType is not supported by WinRT
            return type;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        public static bool IsSerializable(this Type type)
        {
            return type.GetTypeInfo().IsSerializable;
        }

        public static Type GetBaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }
        
        internal static ConstructorInfo GetConstructor(this Type type, BindingFlags bindingAttr, /*Binder*/object binder, Type[] types, /*ParameterModifier[]*/object modifiers)
        {
            if (!ReferenceEquals(null, binder)) throw new NotSupportedException("Binder not supported by WinRT");
            if (!ReferenceEquals(null, modifiers)) throw new NotSupportedException("ParameterModifier not supported by WinRT");

            var constructors = type.GetTypeInfo().DeclaredConstructors
                .Filter(bindingAttr)
                .Where(c => ParametersMatch(c, types))
                .ToList();

            switch (constructors.Count)
            {
                case 0:
                    return null;
                case 1:
                    return constructors[0];
                default:
                    throw new AmbiguousMatchException("More than one construtor found matching the specified binding constraints. (Note: binding flags are not supported by WinRT)");
            }
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit);
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttributes<T>(inherit);
        }

        public static IEnumerable<MemberInfo> GetMember(this Type type, string name, MemberTypes memberType, BindingFlags bindingAttr)
        {
            // Note: binding flags are simply ignored
            var members = new List<MemberInfo>();

            if ((memberType & MemberTypes.Constructor) == MemberTypes.Constructor)
            {
                members.AddRange(type.GetTypeInfo().DeclaredConstructors.Where(x => string.Equals(x.Name, name, StringComparison.Ordinal)));
            }

            if ((memberType & MemberTypes.Field) == MemberTypes.Field)
            {
                members.Add(type.GetTypeInfo().GetDeclaredField(name));
            }

            if ((memberType & MemberTypes.Method) == MemberTypes.Method)
            {
                members.AddRange(type.GetTypeInfo().GetDeclaredMethods(name));
            }

            if ((memberType & MemberTypes.Property) == MemberTypes.Property)
            {
                members.Add(type.GetTypeInfo().GetDeclaredProperty(name));
            }

            return members.ToArray();
        }

        private static bool ParametersMatch(MethodBase m, Type[] types)
        {
            var parameters = m.GetParameters();
            if (parameters.Length == types.Length)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].ParameterType != types[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private static IEnumerable<T> Filter<T>(this IEnumerable<T> memberInfos, BindingFlags bindingAttr) where T : MemberInfo
        {
            // Note: binding flags are simply ignored
            return memberInfos;
        }
    }
}

#endif