using BusinessLogic.API;
using Microsoft.Win32;

namespace WpfGUI
{
    internal class FileDialog : IFilePathGetter
    {
        public string GetFilePath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            return fileDialog.ShowDialog() == true ? fileDialog.FileName : string.Empty;
        }
    }
}