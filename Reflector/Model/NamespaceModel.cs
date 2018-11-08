using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflector.Model
{
   
    public class NamespaceModel
    {
        
        public string Name { get; set; }
        
        public List<TypeModel> Types { get; set; }

        public NamespaceModel(string name, List<Type> types)
        {
            Name = name;
            Types = types.OrderBy(t => t.Name).Select(t => new TypeModel(t)).ToList();
        }

    }
}
