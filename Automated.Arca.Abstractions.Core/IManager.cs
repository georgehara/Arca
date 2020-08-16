using System;
using System.Collections.Generic;
using System.Reflection;

namespace Automated.Arca.Abstractions.Core
{
	public interface IManager : IProcessable, IExtensionDependencyProvider
	{
		IManagerStatistics Statistics { get; }

		IManager AddAssembly( Assembly assembly );
		IManager AddAssemblyFromFile( string assemblyFile );
		IManager AddAssemblyContainingType( Type type );
		IManager AddAssemblyContainingType<T>();
		IManager AddEntryAssembly();
		IManager AddAssembliesLoadedInProcess();
		IManager AddExtensionDependency( Type baseType, IExtensionDependency baseTypeImplementation );
		IManager AddExtensionDependency<BaseType>( IExtensionDependency baseTypeImplementation );
		IManager Register();
		IManager Configure();
		IEnumerable<Type> GetPriorityTypes();
	}
}
