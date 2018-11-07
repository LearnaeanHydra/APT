using BusinessLogic.Services;
using BusinessLogic.ViewModel;
using Logging;
using System.Windows;

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

            DataContext = new MainViewModel(
                new FileDialog(logger),
                logger,
                new Reflector.Reflector(logger),
                new MetadataItemMapper());
        }
    }
}