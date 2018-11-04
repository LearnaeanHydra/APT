using System;
using System.Collections.Generic;
using System.Linq;
using DataContract.Model;

namespace Reflector.Model
{
    internal static class NamespaceLoader
    {
        internal static NamespaceMetadataDto LoadNamespaceMetadata(string name, IEnumerable<Type> types, AssemblyMetadataStorage metaStore)
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

            metaStore.NamespacesDictionary.Add(namespaceMetadata.NamespaceName, namespaceMetadata);

            namespaceMetadata.Types = (from type in types orderby type.Name select TypeLoader.LoadTypeMetadataDto(type, metaStore)).ToList();

            return namespaceMetadata;
        }
    }
}