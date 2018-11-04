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

        public void When_Map(AssemblyMetadataStorage store)
        {
            try
            {
                Task.Run(() => { _rootItem = _context.Map(store); }).Wait();
            }
            catch (AggregateException)
            {
            }
        }

        [Test]
        public void And_OnlyAssembly()
        {
            AssemblyMetadataDto assemblyMetadata = new AssemblyMetadataDto()
            {
                Id = "TestAssembly",
                Name = "TestAssembly",
                Namespaces = new List<NamespaceMetadataDto>()
            };
            AssemblyMetadataStorage storage = new AssemblyMetadataStorage(assemblyMetadata);

            When_Map(storage);

            Then_TreeShouldBe(new MetadataItem("TestAssembly", false));

        }

        public void Then_TreeShouldBe(MetadataItem correctRoot)
        {
            _rootItem.Should().BeEquivalentTo(correctRoot);
        }
    }
}
