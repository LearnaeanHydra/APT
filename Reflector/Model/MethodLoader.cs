using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DataContract.Model;
using DataContract.Model.Enums;
using Reflector.ExtensionMethods;

namespace Reflector
{
    public partial class Reflector
    {
        internal MethodMetadataDto LoadMethodMetadataDto(MethodBase method, AssemblyMetadataStorage metaStore)
        {
            if (method == null)
            {
                throw new ArgumentNullException($"{nameof(method)} argument is null.");
            }

            MethodMetadataDto methodMetadataDto = new MethodMetadataDto()
            {
                Name = method.Name,
                Modifiers = EmitModifiers(method),
                Extension = IsExtension(method)
            };

            methodMetadataDto.GenericArguments = !method.IsGenericMethodDefinition ? new List<TypeMetadataDto>() : EmitGenericArguments(method.GetGenericArguments(), metaStore);
            methodMetadataDto.ReturnType = EmitReturnType(method, metaStore);
            methodMetadataDto.Parameters = EmitParameters(method.GetParameters(), metaStore).ToList();

            string parameters = methodMetadataDto.Parameters.Any()
                ? methodMetadataDto.Parameters.Select(methodInstance => methodInstance.Name)
                    .Aggregate((current, next) => current + ", " + next)
                : "none";

            string generics = methodMetadataDto.GenericArguments.Any()
                ? methodMetadataDto.GenericArguments.Select(typeInstance => typeInstance.Id)
                    .Aggregate((c, n) => $"{c}, {n}")
                : "none";

            methodMetadataDto.Id = $"{method.DeclaringType.FullName}{method.Name} args {parameters} generics {generics} declaredBy {method.DeclaringType.FullName}";

            if (!metaStore.MethodsDictionary.ContainsKey(methodMetadataDto.Id))
            {
                _logger.Trace("Adding method to dictionary: Id =" + methodMetadataDto.Id);
                metaStore.MethodsDictionary.Add(methodMetadataDto.Id, methodMetadataDto);
                return methodMetadataDto;
            }
            else
            {
                _logger.Trace("Using method already added to dictionary: Id =" + methodMetadataDto.Id);
                return metaStore.MethodsDictionary[methodMetadataDto.Id];
            }
        }

        internal IEnumerable<MethodMetadataDto> EmitMethods(IEnumerable<MethodBase> methods, AssemblyMetadataStorage metaStore)
        {
            return (from MethodBase currentMethod in methods
                    where currentMethod.IsVisible()
                    select LoadMethodMetadataDto(currentMethod, metaStore)).ToList();
        }

        private IEnumerable<ParameterMetadataDto> EmitParameters(IEnumerable<ParameterInfo> parameters, AssemblyMetadataStorage metaStore)
        {
            List<ParameterMetadataDto> parametersMetadata = new List<ParameterMetadataDto>();
            foreach (var parameter in parameters)
            {
                string id = $"{parameter.ParameterType.FullName}.{parameter.Name}";
                if (metaStore.ParametersDictionary.ContainsKey(id))
                {
                    _logger.Trace("Using parameter already added to dictionary: Id =" + id);
                    parametersMetadata.Add(metaStore.ParametersDictionary[id]);
                }
                else
                {
                    ParameterMetadataDto newParameter = new ParameterMetadataDto(parameter.Name, LoadTypeMetadataDto(parameter.ParameterType, metaStore));
                    newParameter.Id = id;
                    metaStore.ParametersDictionary.Add(id, newParameter);
                    _logger.Trace("Adding parameter to dictionary: Id =" + id);
                    parametersMetadata.Add(newParameter);
                }
            }

            return parametersMetadata;
        }

        private TypeMetadataDto EmitReturnType(MethodBase method, AssemblyMetadataStorage metaStore)
        {
            MethodInfo methodInfo = method as MethodInfo;
            return methodInfo == null ? null : LoadTypeMetadataDto(methodInfo.ReturnType, metaStore);
        }

        private static bool IsExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }

        private Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            AccessLevel access = AccessLevel.IsPrivate;
            if (method.IsPublic)
            {
                access = AccessLevel.IsPublic;
            }
            else if (method.IsFamily)
            {
                access = AccessLevel.IsProtected;
            }
            else if (method.IsFamilyAndAssembly)
            {
                access = AccessLevel.IsProtectedInternal;
            }

            AbstractEnum isAbstract = AbstractEnum.NotAbstract;
            if (method.IsAbstract)
            {
                isAbstract = AbstractEnum.Abstract;
            }

            StaticEnum isStatic = StaticEnum.NotStatic;
            if (method.IsStatic)
            {
                isStatic = StaticEnum.Static;
            }

            VirtualEnum isVirtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
            {
                isVirtual = VirtualEnum.Virtual;
            }

            return new Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum>(access, isAbstract, isStatic, isVirtual);
        }
    }
}