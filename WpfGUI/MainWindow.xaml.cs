using System.Windows;
using BusinessLogic.ViewModel;
using Logging;

namespace WpfGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Logger logger = new Logger();

            DataContext = new MainViewModel()
            {
                Logger = logger,
                PathLoader = new FileDialog(logger)
            };
        }
    }
}