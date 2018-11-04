using System.Windows;
using BusinessLogic.Services;
using BusinessLogic.ViewModel;

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

            DataContext = new MainViewModel(
                new FileDialog(),
                new Reflector.Reflector(),
                new MetadataItemMapper()
            );
        }
    }
}