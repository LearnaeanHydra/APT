using System;
using System.Collections.Generic;
using DataContract.Model.Enums;

namespace DataContract.Model
{
    public class MethodMetadataDto : BaseMetadataDto
    {
        public string Name { get; set; }

        public IEnumerable<TypeMetadataDto> GenericArguments { get; set; }

        public Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }

        public TypeMetadataDto ReturnType { get; set; }

        public bool Extension { get; set; }

        public IEnumerable<ParameterMetadataDto> Parameters { get; set; }
    }
}