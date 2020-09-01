using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForIntegrationEventHandlerRegistryAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( IntegrationEventHandlerRegistryAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IIntegrationEventHandlerRegistry );

		public ExtensionForIntegrationEventHandlerRegistryAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.InstantiatePerContainer( BaseInterfaceOfTypeWithAttribute!, typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
