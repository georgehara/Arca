using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	public class ConfigurationContext : ProcessingContext, IConfigurationContext
	{
		public ConfigurationContext( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
