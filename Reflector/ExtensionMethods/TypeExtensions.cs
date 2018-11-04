using System;

namespace Reflector.ExtensionMethods
{
    internal static class TypeExtensions
    {
        internal static bool IsVisible(this Type type)
        {
            return type.IsPublic || type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamANDAssem;
        }

        internal static string GetNamespace(this Type type)
        {
            string ns = type.Namespace;
            return ns ?? string.Empty;
        }
    }
}
