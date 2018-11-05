using System.Windows.Input;

namespace CommandLineGUI.FrameworkElements
{
    public class MenuItem
    {
        public string Header { get; set; }

        public ICommand Command { get; set; }
    }
}
