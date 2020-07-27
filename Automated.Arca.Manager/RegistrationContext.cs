using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	public class RegistrationContext : ProcessingContext, IRegistrationContext
	{
		public RegistrationContext( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
