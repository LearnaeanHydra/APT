using System.Collections.ObjectModel;
using DataContract.Enums;
using Reflector.Model;

namespace BusinessLogic.Model
{
    public class NamespaceTreeItem : TreeViewItem
    {
        private readonly NamespaceModel _namespaceModel;

        public NamespaceTreeItem(NamespaceModel namespaceModel)
            : base(namespaceModel.Name, ItemTypeEnum.Namespace)
        {
            _namespaceModel = namespaceModel;
        }

        protected override void BuildTreeView(ObservableCollection<TreeViewItem> children)
        {
            if (_namespaceModel?.Types != null)
            {
                foreach (TypeModel typeModel in _namespaceModel?.Types)
                {
                    ItemTypeEnum typeEnum = typeModel.Type == TypeKind.Class ?
                        ItemTypeEnum.Class : typeModel.Type == TypeKind.Enum ?
                            ItemTypeEnum.Enum : typeModel.Type == TypeKind.Interface ?
                                ItemTypeEnum.Interface : ItemTypeEnum.Struct;

                    children.Add(new TypeTreeItem(TypeModel.TypeDictionary[typeModel.Name], typeEnum));
                }
            }
        }
    }
}
