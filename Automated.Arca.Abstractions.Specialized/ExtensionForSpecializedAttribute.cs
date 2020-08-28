using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;

namespace Automated.Arca.Abstractions.Specialized
{
	public abstract class ExtensionForSpecializedAttribute : ExtensionForProcessableAttribute
	{
		protected IDependencyInjectionProxy D => X<IDependencyInjectionProxy>();
		protected ISpecializedProxy S => X<ISpecializedProxy>();

		public ExtensionForSpecializedAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
