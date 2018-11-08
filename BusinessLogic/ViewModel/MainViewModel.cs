using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using BusinessLogic.API;
using BusinessLogic.Base;
using BusinessLogic.Model;
using DataContract.API;

namespace BusinessLogic.ViewModel
{
    public class MainViewModel : BindableBase
    {
        public ICommand LoadMetadataCommand { get; }

        public IFilePathGetter PathLoader { get; set; }

        public ILogger Logger { get; set; }

        private Reflector.Reflector _reflector;

        private string _pathVariable;

        public ObservableCollection<TreeViewItem> HierarchicalAreas { get; set; }

        public string PathVariable
        {
            get => _pathVariable;
            set => SetProperty(ref _pathVariable, value);
        }

        public MainViewModel()
        {
            HierarchicalAreas = new ObservableCollection<TreeViewItem>();
            LoadMetadataCommand = new RelayCommand(Open);
        }

        private void Open()
        {
            string path = PathLoader.GetFilePath();
            if (path == null || !path.Contains(".dll")) return;
            PathVariable = path;
            try
            {
                _reflector = new Reflector.Reflector(Assembly.LoadFrom(PathVariable));
            }
            catch (Exception)
            {
                // ignored
            }

            // TODO clear?
            HierarchicalAreas.Add(new AssemblyTreeItem(_reflector.AssemblyModel));
        }
    }
}
