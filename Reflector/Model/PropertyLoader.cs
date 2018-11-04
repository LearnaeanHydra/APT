using System.Collections.Generic;
using System.Reflection;
using DataContract.Model;
using Reflector.ExtensionMethods;

namespace Reflector
{
    public partial class Reflector
    {
        internal IEnumerable<PropertyMetadataDto> EmitProperties(IEnumerable<PropertyInfo> props, AssemblyMetadataStorage metaStore)
        {
            List<PropertyMetadataDto> properties = new List<PropertyMetadataDto>();
            foreach (PropertyInfo property in props)
            {
                if (property.GetGetMethod().IsVisible() || property.GetSetMethod().IsVisible())
                {
                    string id = $"{property.DeclaringType.FullName}.{property.Name}";
                    if (metaStore.PropertiesDictionary.ContainsKey(id))
                    {
                        _logger.Trace("Using property already added to dictionary: Id =" + id);
                        properties.Add(metaStore.PropertiesDictionary[id]);
                    }
                    else
                    {
                        PropertyMetadataDto newProperty = new PropertyMetadataDto()
                        {
                            Id = id,
                            Name = property.Name
                        };
                        _logger.Trace("Adding new property to dictionary: " + newProperty.Id +" ;Name = " + newProperty.Name);
                        metaStore.PropertiesDictionary.Add(newProperty.Id, newProperty);
                        properties.Add(newProperty);

                        newProperty.TypeMetadata = LoadTypeMetadataDto(property.PropertyType, metaStore);
                    }
                }
            }

            return properties;
        }
    }
}