using BusinessLogic.API;
using DataContract.API;
using Microsoft.Win32;

namespace WpfGUI
{
    internal class FileDialog : IFilePathGetter
    {
        private ILogger _logger;

        public FileDialog(ILogger logger)
        {
            _logger = logger;
        }

        public string GetFilePath()
        {
            _logger.Trace("Looking for file");
            OpenFileDialog fileDialog = new OpenFileDialog();

            string result;
            if (fileDialog.ShowDialog() == true)
            {
               result = fileDialog.FileName;
                _logger.Trace("Opening file: " + result);
            }
            else
            {
                result = string.Empty;
                _logger.Trace("No file has been chosen");
            }

            return result;
        }
    }
}