using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public abstract class ExtensionForProcessableAttribute : DependencyInjection.RegistrationAndConfigurationBase,
		IExtensionForProcessableAttribute
	{
		public abstract Type AttributeType { get; }

		public abstract void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
		public abstract void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute );

		protected IInstantiationRegistry SpecializedRegistry( IRegistrationContext context )
		{
			return GetExtensionDependency<IInstantiationRegistry>( context );
		}
	}
}
