using System;
using System.Linq;
using System.Reflection;
using DataContract.Model;
using Reflector.ExtensionMethods;

namespace Reflector
{
    public partial class Reflector
    {

        internal AssemblyMetadataStorage LoadAssemblyMetadata(Assembly assembly)
        {
            AssemblyMetadataDto assemblyMetadata = new AssemblyMetadataDto()
            {
                Id = assembly.ManifestModule.FullyQualifiedName,
                Name = assembly.ManifestModule.Name,               
            };

            AssemblyMetadataStorage metaStore = new AssemblyMetadataStorage(assemblyMetadata);

            assemblyMetadata.Namespaces = (from Type type in assembly.GetTypes()
                where type.IsVisible()
                group type by type.GetNamespace() into namespaceGroup
                orderby namespaceGroup.Key
                select LoadNamespaceMetadata(namespaceGroup.Key, namespaceGroup, metaStore)).ToList();

            return metaStore;
        }
    }
}