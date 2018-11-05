using System;
using BusinessLogic.Base;
using BusinessLogic.ViewModel;
using CommandLineGUI.FrameworkElements;

namespace CommandLineGUI
{
    public class MainView : FrameworkElement
    {
        private readonly MainViewModel _viewModel;

        public Menu Menu { get; } = new Menu();

        public ConsoleTreeView ConsoleTreeView { get; } = new ConsoleTreeView();

        public MainView(MainViewModel viewModel)
        {
            DataContext = viewModel;
            _viewModel = viewModel;
            SetBinding("Name");
            Menu.Add(new MenuItem() { Command = _viewModel.GetFilePathCommand, Header = "1. Provide file path for .dll" });
            Menu.Add(new MenuItem() { Command = _viewModel.LoadMetadataCommand, Header = "2. Load metadata of the chosen .dll" });
            Menu.Add(new MenuItem() { Command = new RelayCommand(() =>
            {
                ConsoleTreeView.MetadataItems = _viewModel.TreeItems;
                ConsoleTreeView.Display();
            }), Header = "3. Display .dll metadata" });
            Menu.Add(new MenuItem() { Command = new RelayCommand(() => Environment.Exit(0)), Header = "q. Quit" });
        }

        public void Display()
        {
            while (true)
            {
                Menu.Print();
                Menu.InputLoop();
            }
        }
    }
}
