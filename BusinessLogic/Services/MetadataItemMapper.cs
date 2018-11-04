using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Model;
using DataContract.API;
using DataContract.Model;

namespace BusinessLogic.Services
{
    public class MetadataItemMapper : IMapper<AssemblyMetadataStorage, MetadataItem>
    {
        public MetadataItem Map(AssemblyMetadataStorage objectToMap)
        {
            if (objectToMap == null)
            {
                throw new ArgumentNullException($"{nameof(objectToMap)} argument is null.");
            }

            Dictionary<string, MetadataItem> instances = new Dictionary<string, MetadataItem>();
            List<Relation> relations = new List<Relation>();

            // assembly
            bool hasChildren = objectToMap.AssemblyMetadata?.Namespaces.Any() == true;
            MetadataItem assemblyItem = new MetadataItem(objectToMap.AssemblyMetadata.Id, hasChildren);
            instances.Add(assemblyItem.Name, assemblyItem);

            ProcessNamespaceItems(objectToMap, instances, relations, assemblyItem);

            ProcessMutlipleRelationItems(objectToMap.MethodsDictionary, instances, relations, GetRelations, MapItem);
            ProcessMutlipleRelationItems(objectToMap.TypesDictionary, instances, relations, GetRelations, MapItem);
            ProcessSingleRelationItems(objectToMap.ParametersDictionary, instances, relations, GetRelation, MapItem);
            ProcessSingleRelationItems(objectToMap.PropertiesDictionary, instances, relations, GetRelation, MapItem);

            // lets get fornicating
            foreach (var relation in relations)
            {
                instances[relation.Parent].Children.Add(instances[relation.Child]);
            }

            return assemblyItem;
        }

        private void ProcessNamespaceItems(
            AssemblyMetadataStorage objectToMap,
            Dictionary<string, MetadataItem> instances,
            List<Relation> relations,
            MetadataItem assemblyItem)
        {
            foreach (var namespaceMetadataDto in objectToMap.NamespacesDictionary)
            {
                MetadataItem item = MapItem(namespaceMetadataDto.Value);
                foreach (var relation in GetRelations(namespaceMetadataDto.Value))
                {
                    relations.Add(relation);
                }

                relations.Add(new Relation(assemblyItem.Name, item.Name));
                instances.Add(item.Name, item);
            }
        }

        private void ProcessSingleRelationItems<T>(
            Dictionary<string, T> itemsDictionary,
            Dictionary<string, MetadataItem> instances,
            List<Relation> relations,
            Func<T, Relation> relationFunction,
            Func<T, MetadataItem> mapFunction)
        {
            foreach (var dictItem in itemsDictionary)
            {
                MetadataItem item = mapFunction(dictItem.Value);
                relations.Add(relationFunction(dictItem.Value));
                instances.Add(dictItem.Key, item);
            }
        }

        private void ProcessMutlipleRelationItems<T>(
            Dictionary<string, T> itemsDictionary,
            Dictionary<string, MetadataItem> instances,
            List<Relation> relations,
            Func<T, IEnumerable<Relation>> relationFunction,
            Func<T, MetadataItem> mapFunction)
        {
            foreach (var dictItem in itemsDictionary)
            {
                MetadataItem item = mapFunction(dictItem.Value);
                foreach (var relation in relationFunction(dictItem.Value))
                {
                    relations.Add(relation);
                }

                instances.Add(dictItem.Key, item);
            }
        }

        private Relation GetRelation(PropertyMetadataDto value)
        {
            return new Relation(value.Id, value.TypeMetadata.Id);
        }

        private MetadataItem MapItem(PropertyMetadataDto value)
        {
            return new MetadataItem($"Property: {value.Name}", true);
        }

        private Relation GetRelation(ParameterMetadataDto value)
        {
            return new Relation(value.Id, value.TypeMetadata.Id);
        }

        private MetadataItem MapItem(ParameterMetadataDto value)
        {
            return new MetadataItem($"Parameter: {value.Name}", true);
        }

        private IEnumerable<Relation> GetRelations(NamespaceMetadataDto value)
        {
            foreach (var item in value.Types)
            {
                yield return new Relation($"Namespace: {value.Id}", item.Id);
            }
        }

        private MetadataItem MapItem(NamespaceMetadataDto value)
        {
            return new MetadataItem($"Namespace: {value.Id}", true);
        }

        private IEnumerable<Relation> GetRelations(TypeMetadataDto value)
        {
            foreach (var item in value.Constructors)
            {
                yield return new Relation(value.Id, item.Id);
            }

            foreach (var item in value.Methods)
            {
                yield return new Relation(value.Id, item.Id);
            }

            foreach (var item in value.Properties)
            {
                yield return new Relation(value.Id, item.Id);
            }

            // foreach (var item in value.Attributes)
            // {
            //    yield return new Relation(value.Id, );
            // }
            // TODO check attributes

            foreach (var item in value.GenericArguments)
            {
                yield return new Relation(value.Id, item.Id);
            }

            foreach (var item in value.ImplementedInterfaces)
            {
                yield return new Relation(value.Id, item.Id);
            }

            foreach (var item in value.NestedTypes)
            {
                yield return new Relation(value.Id, item.Id);
            }
        }

        private MetadataItem MapItem(TypeMetadataDto objectToMap)
        {
            return new MetadataItem(
                $"{objectToMap.TypeKind.ToString().Replace("Type", string.Empty)}: {objectToMap.TypeName}", true);
        }

        private IEnumerable<Relation> GetRelations(MethodMetadataDto parent)
        {
            foreach (var argument in parent.GenericArguments)
            {
                yield return new Relation(parent.Id, argument.Id);
            }

            foreach (var parameter in parent.Parameters)
            {
                yield return new Relation(parent.Id, parameter.Id);
            }

            if (parent.ReturnType != null) yield return new Relation(parent.Id, parent.ReturnType.Id);
        }

        private MetadataItem MapItem(MethodMetadataDto objectToMap)
        {
            bool hasChildren =
                objectToMap.GenericArguments.Any() ||
                objectToMap.Parameters.Any();

            return new MetadataItem(
                $"{objectToMap.Modifiers.Item1} " +
                $"{objectToMap.ReturnType?.TypeName ?? "void"} " +
                $"{objectToMap.Name} ",
                hasChildren);
        }
    }
}