using DataContract.Model;
using DataContract.Model.Enums;
using Reflector.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflector
{
    public partial class Reflector
    {
        internal TypeMetadataDto LoadTypeMetadataDto(Type type, AssemblyMetadataStorage metaStore)
        {
            if (type == null)
            {
                throw new ArgumentNullException($"{nameof(type)} argument is null.");
            }

            if (type.IsGenericParameter)
            {
                return LoadGenericParameterTypeObject(type, metaStore);

            }
            else
            {
                if (!metaStore.TypesDictionary.ContainsKey(type.FullName))
                {
                    TypeMetadataDto metadataType;

                    // if type is not declared in assembly being inspected
                    if (type.Assembly.ManifestModule.FullyQualifiedName != metaStore.AssemblyMetadata.Id) // load basic info
                    {
                        metadataType = LoadSimpleTypeObject(type, metaStore);
                    }
                    else // load full type information
                    {
                        metadataType = LoadFullTypeObject(type, metaStore);
                    }

                    return metadataType;
                }
                else
                {
                    _logger.Trace("Using type already added to dictionary with key: " + type.FullName);
                    return metaStore.TypesDictionary[type.FullName];

                }
            }
        }

        private TypeMetadataDto LoadGenericParameterTypeObject(Type type, AssemblyMetadataStorage metaStore)
        {
            TypeMetadataDto metadataType;
            string id = $"{type.DeclaringType.FullName}<{type.Name}>";

            _logger.Trace("Adding generic argument type with Id =" + id);

            if (!metaStore.TypesDictionary.ContainsKey(id))
            {
                metadataType = new TypeMetadataDto()
                {
                    Id = id,
                    TypeName = type.Name,
                    NamespaceName = type.Namespace,
                    Modifiers = null,
                    TypeKind = GetTypeKind(type),
                    Attributes = new List<Attribute>(),
                    Properties = new List<PropertyMetadataDto>(),
                    Constructors = new List<MethodMetadataDto>(),
                    ImplementedInterfaces = new List<TypeMetadataDto>(),
                    Methods = new List<MethodMetadataDto>(),
                    NestedTypes = new List<TypeMetadataDto>(),
                    GenericArguments = new List<TypeMetadataDto>()
                };
                metaStore.TypesDictionary.Add(metadataType.Id, metadataType);
                return metadataType;
            }
            else
            {
                return metaStore.TypesDictionary[id];
            }
        }

        private TypeMetadataDto LoadSimpleTypeObject(Type type, AssemblyMetadataStorage metaStore)
        {
            TypeMetadataDto metadataType;
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
                ImplementedInterfaces = new List<TypeMetadataDto>(),
                Methods = new List<MethodMetadataDto>(),
                NestedTypes = new List<TypeMetadataDto>()
            };

            metaStore.TypesDictionary.Add(type.FullName, metadataType);

            if (type.IsGenericTypeDefinition)
            {
                metadataType.GenericArguments = EmitGenericArguments(type.GetGenericArguments(), metaStore);
                metadataType.TypeName =
                    $"{type.Name}<{metadataType.GenericArguments.Select(a => a.TypeName).Aggregate((p, n) => $"{p}, {n}")}>";
            }
            else
            {
                metadataType.GenericArguments = new List<TypeMetadataDto>();
            }

            _logger.Trace("Adding type not declared in assembly being inspected: Id =" + metadataType.Id + " ; Name = " + metadataType.TypeName);
            return metadataType;
        }

        private TypeMetadataDto LoadFullTypeObject(Type type, AssemblyMetadataStorage metaStore)
        {
            TypeMetadataDto metadataType = new TypeMetadataDto()
            {
                Id = type.FullName,
                TypeName = type.Name,
                NamespaceName = type.Namespace,
                Modifiers = EmitModifiers(type),
                TypeKind = GetTypeKind(type),
                Attributes = type.GetCustomAttributes(false).Cast<Attribute>()
            };
            _logger.Trace("Adding type: Id =" + metadataType.Id + " ; Name = " + metadataType.TypeName);
            metaStore.TypesDictionary.Add(type.FullName, metadataType);

            metadataType.DeclaringType = EmitDeclaringType(type.DeclaringType, metaStore);
            metadataType.ImplementedInterfaces = EmitImplements(type.GetInterfaces(), metaStore);
            metadataType.BaseType = EmitExtends(type.BaseType, metaStore);
            metadataType.NestedTypes = EmitNestedTypes(type.GetNestedTypes(), metaStore);

            if (type.IsGenericTypeDefinition)
            {
                metadataType.GenericArguments = EmitGenericArguments(type.GetGenericArguments(), metaStore);
                metadataType.TypeName =
                    $"{type.Name}<{metadataType.GenericArguments.Select(a => a.TypeName).Aggregate((p, n) => $"{p}, {n}")}>";
            }
            else
            {
                metadataType.GenericArguments = new List<TypeMetadataDto>();
            }

            metadataType.Constructors = EmitMethods(type.GetConstructors(), metaStore);
            metadataType.Methods = EmitMethods(type.GetMethods(BindingFlags.DeclaredOnly), metaStore);

            metadataType.Properties = EmitProperties(type.GetProperties(), metaStore);
            return metadataType;
        }

        internal IEnumerable<TypeMetadataDto> EmitGenericArguments(IEnumerable<Type> arguments, AssemblyMetadataStorage metaStore)
        {
            return (from Type argument in arguments select LoadTypeMetadataDto(argument, metaStore)).ToList();
        }

        private TypeMetadataDto EmitExtends(Type baseType, AssemblyMetadataStorage metaStore)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) ||
                baseType == typeof(Enum))
            {
                return null;
            }

            return LoadTypeMetadataDto(baseType, metaStore);
        }

        private TypeMetadataDto EmitDeclaringType(Type declaringType, AssemblyMetadataStorage metaStore)
        {
            if (declaringType == null)
            {
                return null;
            }

            return LoadTypeMetadataDto(declaringType, metaStore);
        }

        private IEnumerable<TypeMetadataDto> EmitNestedTypes(IEnumerable<Type> nestedTypes, AssemblyMetadataStorage metaStore)
        {
            return (from type in nestedTypes
                    where type.IsVisible()
                    select LoadTypeMetadataDto(type, metaStore)).ToList();
        }

        private IEnumerable<TypeMetadataDto> EmitImplements(IEnumerable<Type> interfaces, AssemblyMetadataStorage metaStore)
        {
            return (from currentInterface in interfaces
                    select LoadTypeMetadataDto(currentInterface, metaStore)).ToList();
        }

        private TypeKind GetTypeKind(Type type) // #80 TPA: Reflection - Invalid return value of GetTypeKind()
        {
            return type.IsEnum ? TypeKind.EnumType :
                type.IsValueType ? TypeKind.StructType :
                type.IsInterface ? TypeKind.InterfaceType :
                TypeKind.ClassType;
        }

        private Tuple<AccessLevel, SealedEnum, AbstractEnum> EmitModifiers(Type type)
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