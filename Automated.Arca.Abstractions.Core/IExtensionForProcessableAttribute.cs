using System;

namespace Automated.Arca.Abstractions.Core
{
	public interface IExtensionForProcessableAttribute : IExtensionDependencyProviderContainer
	{
		Type AttributeType { get; }
		Type? BaseInterfaceOfTypeWithAttribute { get; }

		void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
		void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
	}
}
