using System;
using System.Collections.Generic;
using System.Reflection;

namespace Automated.Arca.Abstractions.Core
{
	public interface IManager : IProcessable, IExtensionDependencyProvider
	{
		IManagerStatistics Statistics { get; }

		IManager AddExtensionDependency( Type baseType, IExtensionDependency baseTypeImplementation );
		IManager AddExtensionDependency<BaseType>( IExtensionDependency baseTypeImplementation );
		IManager AddAssembly( Assembly assembly );
		IManager AddAssemblyFromFile( string assemblyFile );
		IManager AddAssemblyContainingType( Type type );
		IManager AddAssemblyContainingType<T>();
		IManager AddEntryAssembly();
		IManager AddCurrentAssembly();
		IManager AddAssembliesLoadedInProcess();
		IManager Register();
		IManager Configure();
		IEnumerable<Type> GetPriorityTypes();
	}
}
