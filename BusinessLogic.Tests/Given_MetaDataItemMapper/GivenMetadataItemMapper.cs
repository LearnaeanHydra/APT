using BusinessLogic.Services;
using DataContract.Model;
using DataContract.Model.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BusinessLogicTests.Given_MetaDataItemMapper
{
    [TestFixture]
    public class GivenMetadataItemMapper
    {
        protected MetadataItemMapper _context;
        protected AssemblyMetadataDto _assemblyMetadata;
        protected AssemblyMetadataStorage _storage;

        protected const string _assemblyName = "TestAssembly";
        protected const string _namespaceName = "TestNamespace";
        protected const string _typeName = "TestType";
        protected const string _secondTypeName = "2TestType";
        protected const string _thirdTypeName = "3TestType";
        protected const string _propertyName = "TestProperty";
        protected const string _methodName = "TestMethod";
        protected const string _parameterName = "TestParameter";
        protected const AccessLevel _typeAccessLevel = AccessLevel.IsPublic;
        protected const AccessLevel _methodAccessLevel = AccessLevel.IsPublic;

        [SetUp]
        public void Given()
        {
            _context = new MetadataItemMapper();
        }

        protected void With_AssemblyMetadata()
        {
            _assemblyMetadata = new AssemblyMetadataDto()
            {
                Id = _assemblyName,
                Name = _assemblyName,
                Namespaces = new List<NamespaceMetadataDto>()
            };

            _storage = new AssemblyMetadataStorage(_assemblyMetadata);
        }


        protected void With_NamespaceMetaData()
        {
            NamespaceMetadataDto namespaceData = new NamespaceMetadataDto()
            {
                Id = _namespaceName,
                NamespaceName = _namespaceName,
                Types = new List<TypeMetadataDto>()
            };

            (_assemblyMetadata.Namespaces as List<NamespaceMetadataDto>).Add(namespaceData);
            _storage.NamespacesDictionary.Add(namespaceData.Id, namespaceData);
        }

        protected void With_TypeMetaData()
        {
            NamespaceMetadataDto namespaceMetadata = _storage.NamespacesDictionary[_namespaceName];
            TypeMetadataDto typeMetadata = CreateSimpleTypeMetadata(_typeName);
            (namespaceMetadata.Types as List<TypeMetadataDto>).Add(typeMetadata);
            _storage.TypesDictionary.Add(typeMetadata.Id, typeMetadata);
        }

        protected void With_PropertyMetadata()
        {
            TypeMetadataDto typeMetadata = _storage.TypesDictionary[_typeName];
            typeMetadata.TypeKind = TypeKind.ClassType;

            TypeMetadataDto propertyTypeMetadata = CreateSimpleTypeMetadata(_secondTypeName);
            PropertyMetadataDto propertyMetadata = new PropertyMetadataDto()
            {
                Id = _propertyName,
                Name = _propertyName,
                TypeMetadata = propertyTypeMetadata
            };
            (typeMetadata.Properties as List<PropertyMetadataDto>).Add(propertyMetadata);
            _storage.PropertiesDictionary.Add(propertyMetadata.Id, propertyMetadata);
            _storage.TypesDictionary.Add(propertyTypeMetadata.Id, propertyTypeMetadata);
        }

        protected void With_VoidMethodMetadata(IEnumerable<ParameterMetadataDto> parameters)
        {
            TypeMetadataDto typeMetadata = _storage.TypesDictionary[_typeName];
            typeMetadata.TypeKind = TypeKind.ClassType;

            MethodMetadataDto methodMetadata = new MethodMetadataDto()
            {
                Id = _methodName,
                Name = _methodName,
                GenericArguments = new List<TypeMetadataDto>(),
                Modifiers = new Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum>(
                    _methodAccessLevel,
                    default(AbstractEnum),
                    default(StaticEnum),
                    default(VirtualEnum)),
                ReturnType = null,
                Parameters = parameters
            };

            foreach (ParameterMetadataDto parameterMetadataDto in parameters)
            {
                _storage.ParametersDictionary.Add(parameterMetadataDto.Id, parameterMetadataDto);   
                _storage.TypesDictionary.Add(parameterMetadataDto.TypeMetadata.Id, parameterMetadataDto.TypeMetadata);
            }

            (typeMetadata.Methods as List<MethodMetadataDto>).Add(methodMetadata);
            _storage.MethodsDictionary.Add(methodMetadata.Id, methodMetadata);
        }

        protected TypeMetadataDto CreateSimpleTypeMetadata(string typeName)
        {
            return new TypeMetadataDto // add only basic information
            {
                Id = typeName,
                TypeName = typeName,
                NamespaceName = _namespaceName,
                Modifiers = new Tuple<AccessLevel, SealedEnum, AbstractEnum>(
                    _typeAccessLevel,
                    default(SealedEnum),
                    default(AbstractEnum)),
                TypeKind = default(TypeKind),
                Attributes = new List<Attribute>(),
                Properties = new List<PropertyMetadataDto>(),
                Constructors = new List<MethodMetadataDto>(),
                GenericArguments = new List<TypeMetadataDto>(),
                ImplementedInterfaces = new List<TypeMetadataDto>(),
                Methods = new List<MethodMetadataDto>(),
                NestedTypes = new List<TypeMetadataDto>()
            };
        }
    }
}
