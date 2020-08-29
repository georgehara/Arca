using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public class SomeComponentForRegistratorConfigurator : ISomeConfigurable
	{
		public bool Configured { get; set; }
	}

	public class SomeForRegistratorConfigurator : DependencyInjectionRegistratorConfigurator
	{
		public SomeForRegistratorConfigurator( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context )
		{
			D.R.InstantiatePerContainer( typeof( SomeComponentForRegistratorConfigurator ), false );
		}

		public override void Configure( IConfigurationContext context )
		{
			var component = D.P.GetRequiredInstance<SomeComponentForRegistratorConfigurator>();

			component.Configured = true;
		}
	}
}
