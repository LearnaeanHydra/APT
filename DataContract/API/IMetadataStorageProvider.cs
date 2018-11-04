using DataContract.Model;

namespace DataContract.API
{
    public interface IMetadataStorageProvider
    {
        AssemblyMetadataStorage GetMetadataStorage(string connectionString);
    }
}
