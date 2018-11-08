using System.Collections.ObjectModel;
using Reflector.Model;

namespace BusinessLogic.Model
{
    public class AssemblyTreeItem : TreeViewItem
    {
        private readonly AssemblyModel _assemblyModel;

        public AssemblyTreeItem(AssemblyModel assembly)
            : base(assembly.Name, ItemTypeEnum.Assembly)
        {
            _assemblyModel = assembly;
        }

        protected override void BuildTreeView(ObservableCollection<TreeViewItem> children)
        {
            if (_assemblyModel?.NamespaceModels != null)
            {
                foreach (NamespaceModel namespaceModel in _assemblyModel.NamespaceModels)
                {
                    children.Add(new NamespaceTreeItem(namespaceModel));
                }
            }
        }
    }
}
