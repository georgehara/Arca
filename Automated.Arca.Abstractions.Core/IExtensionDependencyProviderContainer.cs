using System;

namespace Automated.Arca.Abstractions.Core
{
	public interface IExtensionDependencyProviderContainer
	{
		IExtensionDependencyProvider ExtensionDependencyProvider { get; }
		object X( Type type );
		T X<T>();
	}
}
