using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;

namespace Automated.Arca.Abstractions.Specialized
{
	public abstract class SpecializedRegistratorConfigurator : RegistratorConfigurator
	{
		protected IDependencyInjectionProxy D => X<IDependencyInjectionProxy>();
		protected ISpecializedProxy S => X<ISpecializedProxy>();

		public SpecializedRegistratorConfigurator( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
