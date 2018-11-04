using System.Collections.Generic;

namespace BusinessLogic.Model
{
    public class MetadataItem
    {
        public MetadataItem(string name, bool hasChildren)
        {
            Name = name;
            Children = new List<MetadataItem>();
            IsExpendable = hasChildren;
        }

        public string Name { get; set; }

        public List<MetadataItem> Children { get; }

        public bool IsExpendable { get; set; }
    }
}
