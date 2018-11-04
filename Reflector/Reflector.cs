using System.Reflection;
using DataContract.API;
using DataContract.Model;


namespace Reflector
{
    public partial class Reflector : IMetadataStorageProvider
    {
        private ILogger _logger;

        public Reflector(ILogger logger)
        {
            _logger = logger;
        }

        public AssemblyMetadataStorage GetMetadataStorage(string assemblyFile)
        {
            
            if (string.IsNullOrEmpty(assemblyFile))
            {
                throw new System.ArgumentNullException($"Could not find assembly file such with path: {assemblyFile}");
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            _logger.Trace("Opening assembly: " + assembly.FullName);

            return LoadAssemblyMetadata(assembly);
        }
    }
}
