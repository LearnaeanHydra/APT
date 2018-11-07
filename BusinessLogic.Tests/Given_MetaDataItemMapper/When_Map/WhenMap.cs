using BusinessLogic.Model;
using BusinessLogicTests.Given_MetaDataItemMapper;
using DataContract.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Tests.Given_MetaDataItemMapper.When_Map
{
    public class WhenMap : GivenMetadataItemMapper
    {
        private MetadataItem _rootItem;

        public void When_MapStorage()
        {
            try
            {
                Task.Run(() => { _rootItem = _context.Map(_storage); }).Wait();
            }
            catch (AggregateException)
            {
            }
        }

        [Test]
        public void And_OnlyAssembly()
        {
            With_AssemblyMetadata();

            When_MapStorage();

            Then_TreeShouldBe(new MetadataItem(_assemblyName, false));
        }

        [Test]
        public void And_AssemblyWithEmptyNamespace()
        {
            With_AssemblyMetadata();
            With_NamespaceMetaData();

            When_MapStorage();

            Then_TreeShouldBe(new MetadataItem(_assemblyName, true)
            {
                Children = { new MetadataItem($"Namespace: {_namespaceName}", false)}
            });
        }

        [Test]
        public void And_AssemblyWithNamespaceAndType()
        {
            With_AssemblyMetadata();
            With_NamespaceMetaData();
            With_TypeMetaData();

            When_MapStorage();

            Then_TreeShouldBe(new MetadataItem(_assemblyName, true)
            {
                Children =
                {
                    new MetadataItem($"Namespace: {_namespaceName}", true)
                    {
                        Children = { new MetadataItem($"Enum: {_typeName}", false)}
                    }
                }
            });
        }

        [Test]
        public void And_AssemblyWithNamespaceAndTypeWithProperty()
        {
            With_AssemblyMetadata();
            With_NamespaceMetaData();
            With_TypeMetaData();
            With_PropertyMetadata();

            When_MapStorage();

            Then_TreeShouldBe(new MetadataItem(_assemblyName, true)
            {
                Children =
                {
                    new MetadataItem($"Namespace: {_namespaceName}", true)
                    {
                        Children =
                        {
                            new MetadataItem($"Class: {_typeName}", true)
                            {
                                Children =
                                {
                                    new MetadataItem($"Property: {_propertyName}", true)
                                    {
                                        Children = { new MetadataItem($"Enum: {_secondTypeName}", false)}
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void And_AssemblyWithNamespace_AndTypeWithVoidMethod_WithNoParameters()
        {
            With_AssemblyMetadata();
            With_NamespaceMetaData();
            With_TypeMetaData();
            With_VoidMethodMetadata(new List<ParameterMetadataDto>());

            When_MapStorage();

            Then_TreeShouldBe(new MetadataItem(_assemblyName, true)
            {
                Children =
                {
                    new MetadataItem($"Namespace: {_namespaceName}", true)
                    {
                        Children =
                        {
                            new MetadataItem($"Class: {_typeName}", true)
                            {
                                Children =
                                {
                                    new MetadataItem($"IsPublic void {_methodName}", false)
                                    {
                                       
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void And_AssemblyWithNamespace_AndTypeWithVoidMethod_WithParameters()
        {
            With_AssemblyMetadata();
            With_NamespaceMetaData();
            With_TypeMetaData();
            With_VoidMethodMetadata(new List<ParameterMetadataDto>()
            {
                new ParameterMetadataDto(_parameterName, CreateSimpleTypeMetadata(_thirdTypeName))
                {
                    Id = _parameterName
                }
            });

            When_MapStorage();

            Then_TreeShouldBe(new MetadataItem(_assemblyName, true)
            {
                Children =
                {
                    new MetadataItem($"Namespace: {_namespaceName}", true)
                    {
                        Children =
                        {
                            new MetadataItem($"Class: {_typeName}", true)
                            {
                                Children =
                                {
                                    new MetadataItem($"IsPublic void {_methodName}", true)
                                    {
                                        Children =
                                        {
                                            new MetadataItem($"Parameter: {_parameterName}", true)
                                            {
                                                Children =
                                                {
                                                    new MetadataItem($"Enum: {_thirdTypeName}", false)
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        public void Then_TreeShouldBe(MetadataItem correctRoot)
        {
            _rootItem.Should().BeEquivalentTo(correctRoot);
        }
    }
}
