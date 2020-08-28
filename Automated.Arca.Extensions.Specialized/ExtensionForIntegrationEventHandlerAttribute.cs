using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForIntegrationEventHandlerAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( IntegrationEventHandlerAttribute );

		public ExtensionForIntegrationEventHandlerAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.ToInstantiatePerScope( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (IntegrationEventHandlerAttribute)attribute;

			var eventHandlerRegistry = D.P.GetRequiredInstance<IIntegrationEventHandlerRegistry>();

			eventHandlerRegistry.Add( attributeTyped.EventStreamIdentifier, typeWithAttribute );
		}
	}
}
