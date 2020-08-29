namespace Automated.Arca.Abstractions.Core
{
	public interface IRegistratorConfigurator : IExtensionDependencyProviderContainer
	{
		void Register( IRegistrationContext context );
		void Configure( IConfigurationContext context );
	}
}
