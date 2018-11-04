using System;
using System.Collections.Generic;
using DataContract.Model.Enums;

namespace DataContract.Model
{
    public class TypeMetadataDto : BaseMetadataDto
    {
        public string TypeName { get; set; }

        public string NamespaceName { get; set; }

        public TypeMetadataDto BaseType { get; set; }

        public IEnumerable<TypeMetadataDto> GenericArguments { get; set; }

        public Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }

        public TypeKind TypeKind { get; set; }

        public IEnumerable<Attribute> Attributes { get; set; }

        public IEnumerable<TypeMetadataDto> ImplementedInterfaces { get; set; }

        public IEnumerable<TypeMetadataDto> NestedTypes { get; set; }

        public IEnumerable<PropertyMetadataDto> Properties { get; set; }

        public TypeMetadataDto DeclaringType { get; set; }

        public IEnumerable<MethodMetadataDto> Methods { get; set; }

        public IEnumerable<MethodMetadataDto> Constructors { get; set; }
    }
}