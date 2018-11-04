using BusinessLogic.Services;
using NUnit.Framework;

namespace BusinessLogicTests.Given_MetaDataItemMapper
{
    [TestFixture]
    public class GivenMetadataItemMapper
    {
        protected MetadataItemMapper _context;

        [SetUp]
        public void Given()
        {
            _context = new MetadataItemMapper();
        }
    }
}
