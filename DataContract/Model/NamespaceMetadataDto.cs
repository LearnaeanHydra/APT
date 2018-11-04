using System.Collections.Generic;

namespace DataContract.Model
{
    public class NamespaceMetadataDto : BaseMetadataDto
    {
        public string NamespaceName { get; set; }

        public IEnumerable<TypeMetadataDto> Types { get; set; }
    }
}