using DataContract.Model;
using DataContract.Model.Enums;
using Reflector.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflector.Model
{
    internal static class TypeLoader
    {
        internal static TypeMetadataDto LoadTypeMetadataDto(Type type, AssemblyMetadataStorage metaStore)
        {
            if (type == null)
            {
                throw new ArgumentNullException($"{nameof(type)} argument is null.");
            }

            if (!metaStore.TypesDictionary.ContainsKey(type.FullName))
            {
                TypeMetadataDto metadataType;
                if (type.Assembly.ManifestModule.FullyQualifiedName != metaStore.AssemblyMetadata.Id) // if type is not declared in assembly being inspected
                {
                    metadataType = new TypeMetadataDto // add only basic information
                    {
                        Id = type.FullName,
                        TypeName = type.Name,
                        NamespaceName = type.Namespace,
                        Modifiers = EmitModifiers(type),
                        TypeKind = GetTypeKind(type),
                        Attributes = type.GetCustomAttributes(false).Cast<Attribute>(),
                        Properties = new List<PropertyMetadataDto>(),
                        Constructors = new List<MethodMetadataDto>(),
                        GenericArguments = new List<TypeMetadataDto>(),
                        ImplementedInterfaces = new List<TypeMetadataDto>(),
                        Methods = new List<MethodMetadataDto>(),
                        NestedTypes = new List<TypeMetadataDto>()
                    };

                    metaStore.TypesDictionary.Add(type.FullName, metadataType);
                }
                else // load full type information
                {
                    metadataType = new TypeMetadataDto()
                    {
                        Id = type.FullName,
                        TypeName = type.Name,
                        NamespaceName = type.Namespace,
                        Modifiers = EmitModifiers(type),
                        TypeKind = GetTypeKind(type),
                        Attributes = type.GetCustomAttributes(false).Cast<Attribute>()
                    };

                    metaStore.TypesDictionary.Add(type.FullName, metadataType);

                    metadataType.DeclaringType = EmitDeclaringType(type.DeclaringType, metaStore);
                    metadataType.ImplementedInterfaces = EmitImplements(type.GetInterfaces(), metaStore);
                    metadataType.BaseType = EmitExtends(type.BaseType, metaStore);
                    metadataType.NestedTypes = EmitNestedTypes(type.GetNestedTypes(), metaStore);

                    metadataType.GenericArguments = !type.IsGenericTypeDefinition
                        ? new List<TypeMetadataDto>()
                        : TypeLoader.EmitGenericArguments(type.GetGenericArguments(), metaStore);

                    metadataType.Constructors = MethodLoader.EmitMethods(type.GetConstructors(), metaStore);
                    metadataType.Methods = MethodLoader.EmitMethods(type.GetMethods(BindingFlags.DeclaredOnly), metaStore);

                    metadataType.Properties = PropertyLoader.EmitProperties(type.GetProperties(), metaStore);
                }

                return metadataType;
            }
            else
            {
                return metaStore.TypesDictionary[type.FullName];
            }
        }

        internal static IEnumerable<TypeMetadataDto> EmitGenericArguments(IEnumerable<Type> arguments, AssemblyMetadataStorage metaStore)
        {
            return from Type argument in arguments select LoadTypeMetadataDto(argument, metaStore);
        }

        private static TypeMetadataDto EmitExtends(Type baseType, AssemblyMetadataStorage metaStore)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) ||
                baseType == typeof(Enum))
            {
                return null;
            }

            return LoadTypeMetadataDto(baseType, metaStore);
        }

        private static TypeMetadataDto EmitDeclaringType(Type declaringType, AssemblyMetadataStorage metaStore)
        {
            if (declaringType == null)
            {
                return null;
            }

            return LoadTypeMetadataDto(declaringType, metaStore);
        }

        private static IEnumerable<TypeMetadataDto> EmitNestedTypes(IEnumerable<Type> nestedTypes, AssemblyMetadataStorage metaStore)
        {
            return (from type in nestedTypes
                    where type.IsVisible()
                    select LoadTypeMetadataDto(type, metaStore)).ToList();
        }

        private static IEnumerable<TypeMetadataDto> EmitImplements(IEnumerable<Type> interfaces, AssemblyMetadataStorage metaStore)
        {
            return (from currentInterface in interfaces
                    select LoadTypeMetadataDto(currentInterface, metaStore)).ToList();
        }

        private static TypeKind GetTypeKind(Type type) // #80 TPA: Reflection - Invalid return value of GetTypeKind()
        {
            return type.IsEnum ? TypeKind.EnumType :
                type.IsValueType ? TypeKind.StructType :
                type.IsInterface ? TypeKind.InterfaceType :
                TypeKind.ClassType;
        }

        private static Tuple<AccessLevel, SealedEnum, AbstractEnum> EmitModifiers(Type type)
        {
            AccessLevel accessLevel = AccessLevel.IsPrivate;
            // check if not default
            if (type.IsPublic)
            {
                accessLevel = AccessLevel.IsPublic;
            }
            else if (type.IsNestedPublic)
            {
                accessLevel = AccessLevel.IsPublic;
            }
            else if (type.IsNestedFamily)
            {
                accessLevel = AccessLevel.IsProtected;
            }
            else if (type.IsNestedFamANDAssem)
            {
                accessLevel = AccessLevel.IsProtectedInternal;
            }

            SealedEnum sealedEnum = SealedEnum.NotSealed;
            if (type.IsSealed)
            {
                sealedEnum = SealedEnum.Sealed;
            }

            AbstractEnum abstractEnum = AbstractEnum.NotAbstract;
            if (type.IsAbstract)
            {
                abstractEnum = AbstractEnum.Abstract;
            }

            return new Tuple<AccessLevel, SealedEnum, AbstractEnum>(accessLevel, sealedEnum, abstractEnum);
        }
    }
}