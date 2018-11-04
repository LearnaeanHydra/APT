using System.Collections.Generic;

namespace DataContract.Model
{
    public class AssemblyMetadataDto : BaseMetadataDto
    {
        public string Name { get; set; }

        public IEnumerable<NamespaceMetadataDto> Namespaces { get; set; }
    }
}