using System;

namespace Automated.Arca.Abstractions.Core
{
	public abstract class RegistratorConfigurator : IRegistratorConfigurator
	{
		public IExtensionDependencyProvider ExtensionDependencyProvider { get; }
		public object X( Type type ) => ExtensionDependencyProvider.GetExtensionDependency( type );
		public T X<T>() => ExtensionDependencyProvider.GetExtensionDependency<T>();

		public RegistratorConfigurator( IExtensionDependencyProvider extensionDependencyProvider )
		{
			ExtensionDependencyProvider = extensionDependencyProvider;
		}

		public abstract void Register( IRegistrationContext context );
		public abstract void Configure( IConfigurationContext context );
	}
}
