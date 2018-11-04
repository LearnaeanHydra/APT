using System;
using System.Collections.Generic;
using System.Linq;
using DataContract.Model;

namespace Reflector
{
    public partial class Reflector
    {
        internal NamespaceMetadataDto LoadNamespaceMetadata(string name, IEnumerable<Type> types, AssemblyMetadataStorage metaStore)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} is null/empty/whitespace");
            }

            NamespaceMetadataDto namespaceMetadata = new NamespaceMetadataDto()
            {
                Id = name,
                NamespaceName = name
            };

            _logger.Trace("Adding Namespace to dictionary: " + namespaceMetadata.NamespaceName);
            metaStore.NamespacesDictionary.Add(namespaceMetadata.NamespaceName, namespaceMetadata);

            namespaceMetadata.Types = (from type in types orderby type.Name select LoadTypeMetadataDto(type, metaStore)).ToList();

            return namespaceMetadata;
        }
    }
}