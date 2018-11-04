using System.Reflection;
using DataContract.API;
using DataContract.Model;
using Reflector.Model;

namespace Reflector
{
    public class Reflector : IMetadataStorageProvider
    {
        public AssemblyMetadataStorage GetMetadataStorage(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
            {
                throw new System.ArgumentNullException($"Could not find assembly file such with path: {assemblyFile}");
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFile);

            return AssemblyLoader.LoadAssemblyMetadata(assembly);
        }
    }
}
