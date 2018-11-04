using System;
using System.Collections.Generic;

namespace DataContract.Model
{
    public class AssemblyMetadataStorage
    {
        public AssemblyMetadataDto AssemblyMetadata { get; }

        public Dictionary<string, NamespaceMetadataDto> NamespacesDictionary { get; }

        public Dictionary<string, TypeMetadataDto> TypesDictionary { get; }

        public Dictionary<string, PropertyMetadataDto> PropertiesDictionary { get; }

        public Dictionary<string, MethodMetadataDto> MethodsDictionary { get; }

        public Dictionary<string, ParameterMetadataDto> ParametersDictionary { get; }

        public AssemblyMetadataStorage(AssemblyMetadataDto assemblyMetadata)
        {
            AssemblyMetadata = assemblyMetadata ?? throw new ArgumentNullException(nameof(assemblyMetadata));
            NamespacesDictionary = new Dictionary<string, NamespaceMetadataDto>();
            TypesDictionary = new Dictionary<string, TypeMetadataDto>();
            PropertiesDictionary = new Dictionary<string, PropertyMetadataDto>();
            MethodsDictionary = new Dictionary<string, MethodMetadataDto>();
            ParametersDictionary = new Dictionary<string, ParameterMetadataDto>();
        }
    }
}