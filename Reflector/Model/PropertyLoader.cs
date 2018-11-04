using System.Collections.Generic;
using System.Reflection;
using DataContract.Model;
using Reflector.ExtensionMethods;

namespace Reflector.Model
{
    internal class PropertyLoader
    {
        internal static IEnumerable<PropertyMetadataDto> EmitProperties(IEnumerable<PropertyInfo> props, AssemblyMetadataStorage metaStore)
        {
            List<PropertyMetadataDto> properties = new List<PropertyMetadataDto>();
            foreach (PropertyInfo property in props)
            {
                if (property.GetGetMethod().IsVisible() || property.GetSetMethod().IsVisible())
                {
                    string id = $"{property.DeclaringType.FullName}.{property.Name}";
                    if (metaStore.PropertiesDictionary.ContainsKey(id))
                    {
                        properties.Add(metaStore.PropertiesDictionary[id]);
                    }
                    else
                    {
                        PropertyMetadataDto newProperty = new PropertyMetadataDto()
                        {
                            Id = id,
                            Name = property.Name
                        };
                        metaStore.PropertiesDictionary.Add(newProperty.Id, newProperty);
                        properties.Add(newProperty);

                        newProperty.TypeMetadata = TypeLoader.LoadTypeMetadataDto(property.PropertyType, metaStore);
                    }
                }
            }

            return properties;
        }
    }
}