using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DataContract.Enums;

namespace Reflector.Model
{
   
    public class MethodModel
    {
        public string Name { get; set; }
        
        public List<TypeModel> GenericArguments { get; set; }

        public Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }

        
        public TypeModel ReturnType { get; set; }

        public bool Extension { get; set; }

        public List<ParameterModel> Parameters { get; set; }

        public MethodModel(MethodBase method)
        {
            Name = method.Name;
            GenericArguments = !method.IsGenericMethodDefinition ? null : EmitGenericArguments(method);
            ReturnType = EmitReturnType(method);
            Parameters = EmitParameters(method);
            Modifiers = EmitModifiers(method);
            Extension = EmitExtension(method);
        }

        private List<TypeModel> EmitGenericArguments(MethodBase method)
        {
            return method.GetGenericArguments().Select(t => new TypeModel(t)).ToList();
        }

        public static List<MethodModel> EmitMethods(Type type)
        {
            return type.GetMethods(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Public |
                                   BindingFlags.Static | BindingFlags.Instance).Select(t => new MethodModel(t)).ToList();
        }

        private static List<ParameterModel> EmitParameters(MethodBase method)
        {
            return method.GetParameters().Select(t => new ParameterModel(t.Name,TypeModel.EmitReference(t.ParameterType))).ToList( );
        }

        private static TypeModel EmitReturnType(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            TypeModel.StoreType(methodInfo.ReturnType);
            return TypeModel.EmitReference(methodInfo.ReturnType);
        }

        private static bool EmitExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }

        private static Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            AccessLevel access = method.IsPublic ? AccessLevel.Public :
                method.IsFamily ? AccessLevel.Protected :
                method.IsAssembly ? AccessLevel.Internal : AccessLevel.Private;

            AbstractEnum _abstract = method.IsAbstract ? AbstractEnum.Abstract : AbstractEnum.NotAbstract;

            StaticEnum _static = method.IsStatic ? StaticEnum.Static : StaticEnum.NotStatic;

            VirtualEnum _virtual = method.IsVirtual ? VirtualEnum.Virtual : VirtualEnum.NotVirtual;

            return new Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum>(access, _abstract, _static, _virtual);
        }

        public static List<MethodModel> EmitConstructors(Type type)
        {
            return type.GetConstructors().Select(t => new MethodModel(t)).ToList();
        }
    }
}
