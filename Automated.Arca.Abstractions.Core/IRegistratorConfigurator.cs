namespace Automated.Arca.Abstractions.Core
{
	public interface IRegistratorConfigurator
	{
		void Register( IRegistrationContext context );
		void Configure( IConfigurationContext context );
	}
}
