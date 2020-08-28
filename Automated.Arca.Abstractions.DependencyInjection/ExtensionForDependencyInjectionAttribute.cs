using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class ExtensionForDependencyInjectionAttribute : ExtensionForProcessableAttribute
	{
		protected IDependencyInjectionProxy D => X<IDependencyInjectionProxy>();

		public ExtensionForDependencyInjectionAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
