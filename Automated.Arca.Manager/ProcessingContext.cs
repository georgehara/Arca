using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	public abstract class ProcessingContext : IProcessingContext
	{
		public IExtensionDependencyProvider ExtensionDependencyProvider { get; }

		public ProcessingContext( IExtensionDependencyProvider extensionDependencyProvider )
		{
			ExtensionDependencyProvider = extensionDependencyProvider;
		}

		public object GetExtensionDependency( Type type )
		{
			return ExtensionDependencyProvider.GetExtensionDependency( type );
		}

		public T GetExtensionDependency<T>()
		{
			return (T)ExtensionDependencyProvider.GetExtensionDependency( typeof( T ) );
		}
	}
}
