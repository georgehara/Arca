using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForDomainEventHandlerAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( DomainEventHandlerAttribute );

		public ExtensionForDomainEventHandlerAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.ToInstantiatePerScope( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (DomainEventHandlerAttribute)attribute;

			var eventRegistry = D.P.GetRequiredInstance<IDomainEventRegistry>();
			var eventHandlerRegistry = D.P.GetRequiredInstance<IDomainEventHandlerRegistry>();

			eventRegistry.Add( attributeTyped.EventIdentifier, attributeTyped.EventType );
			eventHandlerRegistry.Add( attributeTyped.EventStreamIdentifier, typeWithAttribute );
		}
	}
}
