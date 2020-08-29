using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	public abstract class ProcessingContext : IProcessingContext
	{
		public IExtensionDependencyProvider ExtensionDependencyProvider { get; }
		public object X( Type type ) => ExtensionDependencyProvider.GetExtensionDependency( type );
		public T X<T>() => ExtensionDependencyProvider.GetExtensionDependency<T>();

		public ProcessingContext( IExtensionDependencyProvider extensionDependencyProvider )
		{
			ExtensionDependencyProvider = extensionDependencyProvider;
		}
	}
}
