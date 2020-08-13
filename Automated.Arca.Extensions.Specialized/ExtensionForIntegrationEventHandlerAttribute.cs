using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForIntegrationEventHandlerAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( IntegrationEventHandlerAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ToInstantiatePerScope( context, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (IntegrationEventHandlerAttribute)attribute;

			var eventHandlerRegistry = Provider( context ).GetRequiredInstance<IIntegrationEventHandlerRegistry>();

			eventHandlerRegistry.Add( attributeTyped.EventStreamIdentifier, typeWithAttribute );
		}
	}
}
