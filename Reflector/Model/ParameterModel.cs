namespace Reflector.Model
{
   
    public class ParameterModel
    {
        
        public string Name { get; set; }
        
        public TypeModel Type { get; set; }

        public ParameterModel(string name, TypeModel typeModel)
        {
            Name = name;
            Type = typeModel;
        }
    }
}
