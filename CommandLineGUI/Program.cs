using BusinessLogic.Services;
using BusinessLogic.ViewModel;
using Logging;

namespace CommandLineGUI
{
    internal class Program
    {
        internal static void Main()
        {
            Logger logger = new Logger();

            MainViewModel vm = new MainViewModel(
                new ConsoleFilePathGetter(),
                logger,
                new Reflector.Reflector(logger),
                new MetadataItemMapper()
            );

            MainView main = new MainView(vm);
            main.Display();
        }
    }
}
