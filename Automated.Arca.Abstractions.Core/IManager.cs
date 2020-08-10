using System;
using System.Reflection;

namespace Automated.Arca.Abstractions.Core
{
	public interface IManager : IProcessable, IExtensionDependencyProvider
	{
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
	}
}
