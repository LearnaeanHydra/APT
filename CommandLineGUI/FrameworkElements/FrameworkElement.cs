using System.Collections.Generic;

namespace CommandLineGUI.FrameworkElements
{
    public abstract class FrameworkElement
    {
        private readonly Dictionary<string, Binding> _bindings = new Dictionary<string, Binding>();

        public object DataContext { get; set; }

        protected void SetBinding(string property)
        {
            Binding binding = new Binding(property)
            {
                Source = DataContext
            };
            binding.Parse();
            _bindings.Add(property, binding);
        }
    }
}
