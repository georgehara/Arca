using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public class SomeComponentForRegistratorConfigurator : ISomeConfigurable
	{
		public bool Configured { get; set; }
	}

	public class SomeForRegistratorConfigurator : RegistrationAndConfigurationBase, IRegistrator, IConfigurator
	{
		public void Register( IRegistrationContext context )
		{
			ToInstantiatePerContainer( context, typeof( SomeComponentForRegistratorConfigurator ) );
		}

		public void Configure( IConfigurationContext context )
		{
			var component = GetRequiredInstance<SomeComponentForRegistratorConfigurator>( context );

			component.Configured = true;
		}
	}
}
