using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForDomainEventHandlerAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( DomainEventHandlerAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ToInstantiatePerScope( context, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (DomainEventHandlerAttribute)attribute;

			var configurationsTyped = Provider( context );

			var eventRegistry = configurationsTyped.GetRequiredInstance<IDomainEventRegistry>();
			var eventHandlerRegistry = configurationsTyped.GetRequiredInstance<IDomainEventHandlerRegistry>();

			eventRegistry.Add( attributeTyped.EventIdentifier, attributeTyped.EventType );
			eventHandlerRegistry.Add( attributeTyped.EventStreamIdentifier, typeWithAttribute );
		}
	}
}
