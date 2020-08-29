using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForDomainEventRegistryAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( DomainEventRegistryAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IDomainEventRegistry );

		public ExtensionForDomainEventRegistryAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (ProcessableWithInterfaceAttribute)attribute;
			var interfaceType = attributeTyped.GetInterfaceOrDefault( typeWithAttribute );

			D.R.InstantiatePerContainer( interfaceType, typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
