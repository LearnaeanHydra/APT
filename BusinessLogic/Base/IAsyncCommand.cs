using System.Threading.Tasks;
using System.Windows.Input;

namespace BusinessLogic.Base
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();

        bool CanExecute();
    }
}
