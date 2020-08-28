using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class DependencyInjectionRegistratorConfigurator : RegistratorConfigurator
	{
		protected IDependencyInjectionProxy D => X<IDependencyInjectionProxy>();

		public DependencyInjectionRegistratorConfigurator( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
