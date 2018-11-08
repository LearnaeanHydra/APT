using System.Collections.ObjectModel;
using DataContract.Enums;
using Reflector.Model;

namespace BusinessLogic.Model
{
    public class MethodTreeItem : TreeViewItem
    {
        public MethodModel MethodModel { get; set; }

        public MethodTreeItem(MethodModel methodModel, ItemTypeEnum type)
            : base(GetModifiers(methodModel) + methodModel.Name, type)
        {
            MethodModel = methodModel;
        }

        public static string GetModifiers(MethodModel model)
        {
            string type = null;
            type += model.Modifiers.Item1.ToString().ToLower() + " ";
            type += model.Modifiers.Item2 == AbstractEnum.Abstract ? AbstractEnum.Abstract.ToString().ToLower() + " " : string.Empty;
            type += model.Modifiers.Item3 == StaticEnum.Static ? StaticEnum.Static.ToString().ToLower() + " " : string.Empty;
            type += model.Modifiers.Item4 == VirtualEnum.Virtual ? VirtualEnum.Virtual.ToString().ToLower() + " " : string.Empty;
            return type;
        }

        protected override void BuildTreeView(ObservableCollection<TreeViewItem> children)
        {
            if (MethodModel.GenericArguments != null)
            {
                foreach (TypeModel genericArgument in MethodModel.GenericArguments)
                {
                    children.Add(new TypeTreeItem(TypeModel.TypeDictionary[genericArgument.Name], ItemTypeEnum.GenericArgument));
                }
            }

            if (MethodModel.Parameters != null)
            {
                foreach (ParameterModel parameter in MethodModel.Parameters)
                {
                    children.Add(new ParameterTreeItem(parameter, ItemTypeEnum.Parameter));
                }
            }

            if (MethodModel.ReturnType != null)
            {
                children.Add(new TypeTreeItem(TypeModel.TypeDictionary[MethodModel.ReturnType.Name], ItemTypeEnum.ReturnType));
            }
        }
    }
}
