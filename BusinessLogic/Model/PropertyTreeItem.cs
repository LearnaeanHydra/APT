using System.Collections.ObjectModel;
using Reflector.Model;

namespace BusinessLogic.Model
{
    public class PropertyTreeItem : TreeViewItem
    {
        public PropertyModel PropertyModel { get; set; }

        public PropertyTreeItem(PropertyModel type, string name)
            : base(name, ItemTypeEnum.Property)
        {
            PropertyModel = type;
        }

        protected override void BuildTreeView(ObservableCollection<TreeViewItem> children)
        {
            if (PropertyModel.Type != null)
            {
                children.Add(new TypeTreeItem(TypeModel.TypeDictionary[PropertyModel.Type.Name], ItemTypeEnum.Type));
            }
        }
    }
}
