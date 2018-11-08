using System.Collections.ObjectModel;
using DataContract.Enums;
using Reflector.Model;

namespace BusinessLogic.Model
{
    public class TypeTreeItem : TreeViewItem
    {
        private readonly TypeModel _typeModel;

        public TypeTreeItem(TypeModel typeModel, ItemTypeEnum type)
            : base(GetModifiers(typeModel) + typeModel.Name, type)
        {
            _typeModel = typeModel;
        }

        public static string GetModifiers(TypeModel model)
        {
            if (model.Modifiers != null)
            {
                string type = null;
                type += model.Modifiers.Item1.ToString().ToLower() + " ";
                type += model.Modifiers.Item2 == SealedEnum.Sealed ? SealedEnum.Sealed.ToString().ToLower() + " " : string.Empty;
                type += model.Modifiers.Item3 == AbstractEnum.Abstract ? AbstractEnum.Abstract.ToString().ToLower() + " " : string.Empty;
                type += model.Modifiers.Item4 == StaticEnum.Static ? StaticEnum.Static.ToString().ToLower() + " " : string.Empty;
                return type;
            }

            return null;
        }

        protected override void BuildTreeView(ObservableCollection<TreeViewItem> children)
        {
            if (_typeModel.BaseType != null)
            {
                children.Add(new TypeTreeItem(TypeModel.TypeDictionary[_typeModel.BaseType.Name], ItemTypeEnum.BaseType));
            }

            if (_typeModel.DeclaringType != null)
            {
                children.Add(new TypeTreeItem(TypeModel.TypeDictionary[_typeModel.DeclaringType.Name], ItemTypeEnum.Type));
            }

            if (_typeModel.Properties != null)
            {
                foreach (PropertyModel propertyModel in _typeModel.Properties)
                {
                    children.Add(new PropertyTreeItem(propertyModel, GetModifiers(propertyModel.Type) + propertyModel.Type.Name + " " + propertyModel.Name));
                }
            }

            if (_typeModel.Fields != null)
            {
                foreach (ParameterModel parameterModel in _typeModel.Fields)
                {
                    children.Add(new ParameterTreeItem(parameterModel, ItemTypeEnum.Field));
                }
            }

            if (_typeModel.GenericArguments != null)
            {
                foreach (TypeModel typeModel in _typeModel.GenericArguments)
                {
                    children.Add(new TypeTreeItem(TypeModel.TypeDictionary[typeModel.Name], ItemTypeEnum.GenericArgument));
                }
            }

            if (_typeModel.ImplementedInterfaces != null)
            {
                foreach (TypeModel typeModel in _typeModel.ImplementedInterfaces)
                {
                    children.Add(new TypeTreeItem(TypeModel.TypeDictionary[typeModel.Name], ItemTypeEnum.InmplementedInterface));
                }
            }

            if (_typeModel.NestedTypes != null)
            {
                foreach (TypeModel typeModel in _typeModel.NestedTypes)
                {
                    ItemTypeEnum type = typeModel.Type == TypeKind.Class ? ItemTypeEnum.NestedClass :
                        typeModel.Type == TypeKind.Struct ? ItemTypeEnum.NestedStructure :
                        typeModel.Type == TypeKind.Enum ? ItemTypeEnum.NestedEnum : ItemTypeEnum.NestedType;
                    children.Add(new TypeTreeItem(TypeModel.TypeDictionary[typeModel.Name], type));
                }
            }

            if (_typeModel.Methods != null)
            {
                foreach (MethodModel methodModel in _typeModel.Methods)
                {
                    children.Add(new MethodTreeItem(methodModel, methodModel.Extension ? ItemTypeEnum.ExtensionMethod : ItemTypeEnum.Method));
                }
            }

            if (_typeModel.Constructors != null)
            {
                foreach (MethodModel methodModel in _typeModel.Constructors)
                {
                    children.Add(new MethodTreeItem(methodModel, ItemTypeEnum.Constructor));
                }
            }
        }
    }
}
