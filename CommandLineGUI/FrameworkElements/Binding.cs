using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CommandLineGUI.FrameworkElements
{
    public class Binding
    {
        private static object Show(object type, string propertyName)
        {
            while (true)
            {
                string[] properties = propertyName.Split('.');
                PropertyInfo propertyInfo;
                if (properties.Length > 1)
                {
                    string property = properties.First();
                    propertyName = propertyName.Substring(property.Length + 1);
                    propertyInfo = type.GetType().GetProperty(propertyName);
                    if (propertyInfo == null)
                    {
                        return $"Data binding error: Unable to get {propertyName} from {type.GetType().FullName}";
                    }

                    type = propertyInfo.GetValue(type);
                    continue;
                }

                propertyInfo = type.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    return $"Data binding error: Unable to get {propertyName} from {type.GetType().FullName}";
                }

                object value = propertyInfo.GetValue(type);
                return value ?? "<<Unset>>";
            }
        }

        public Binding(string property)
        {
            Name = property;
        }

        public object Source { get; set; }

        public string Name { get; set; }

        private static readonly HashSet<INotifyPropertyChanged> _instances = new HashSet<INotifyPropertyChanged>();

        public void Parse()
        {
            object value = Show(Source, Name);
            Console.WriteLine($"{Name} is {value}");
            INotifyPropertyChanged inpc = Source as INotifyPropertyChanged;
            if (inpc == null)
            {
                return;
            }

            if (_instances.Contains(inpc))
            {
                return;
            }

            _instances.Add(inpc);
            inpc.PropertyChanged += Inpc_PropertyChanged;
        }

        private void Inpc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyInfo propertyInfo = Source.GetType().GetProperty(e.PropertyName);
            object value = propertyInfo?.GetValue(Source);
            Console.WriteLine($"{e.PropertyName} changed to {value}");
        }
    }
}
