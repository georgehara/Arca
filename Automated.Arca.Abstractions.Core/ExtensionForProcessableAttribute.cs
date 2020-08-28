using System;

namespace Automated.Arca.Abstractions.Core
{
	public abstract class ExtensionForProcessableAttribute : IExtensionForProcessableAttribute
	{
		public abstract Type AttributeType { get; }
		public virtual Type? BaseInterfaceOfTypeWithAttribute { get; }

		protected IExtensionDependencyProvider ExtensionDependencyProvider { get; }

		protected object X( Type type ) => ExtensionDependencyProvider.GetExtensionDependency( type );
		protected T X<T>() => ExtensionDependencyProvider.GetExtensionDependency<T>();

		public ExtensionForProcessableAttribute( IExtensionDependencyProvider extensionDependencyProvider )
		{
			ExtensionDependencyProvider = extensionDependencyProvider;
		}

		public abstract void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
		public abstract void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
	}
}
