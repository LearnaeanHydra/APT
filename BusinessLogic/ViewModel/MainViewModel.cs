using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using BusinessLogic.API;
using BusinessLogic.Base;
using BusinessLogic.Model;
using DataContract.API;
using DataContract.Model;

namespace BusinessLogic.ViewModel
{
    public class MainViewModel : ValidatableBindableBase
    {
        private readonly IFilePathGetter _filePathGetter;
        private readonly IMetadataStorageProvider _metadataProvider;
        private readonly IMapper<AssemblyMetadataStorage, MetadataItem> _mapper;

        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            set => SetPropertyAndValidate(ref _filePath, value);
        }

        public ICommand GetFilePathCommand { get; }

        public ICommand LoadMetadataCommand { get; }

        private List<MetadataItem> _treeItems;

        public List<MetadataItem> TreeItems
        {
            get => _treeItems;
            set => SetProperty(ref _treeItems, value);
        }

        public MainViewModel(
            IFilePathGetter filePathGetter,
            IMetadataStorageProvider metadataProvider,
            IMapper<AssemblyMetadataStorage, MetadataItem> mapper)
        {
            _filePathGetter = filePathGetter;
            _metadataProvider = metadataProvider;
            _mapper = mapper;
            GetFilePathCommand = new RelayCommand(GetFilePath);
            LoadMetadataCommand = new SimpleAsyncCommand(LoadMetadata);
            TreeItems = new List<MetadataItem>();
        }

        private void GetFilePath()
        {
            FilePath = _filePathGetter.GetFilePath();
        }

        private async Task LoadMetadata()
        {
            Task toDo = new Task(() =>
            {
                AssemblyMetadataStorage storage = _metadataProvider.GetMetadataStorage(FilePath);
                TreeItems = new List<MetadataItem>() { _mapper.Map(storage) };
            });
            toDo.Start();
            await toDo.ConfigureAwait(false);
        }
    }
}