namespace DataContract.Model
{
    public class PropertyMetadataDto : BaseMetadataDto
    {
        public string Name { get; set; }

        public TypeMetadataDto TypeMetadata { get; set; }
    }
}