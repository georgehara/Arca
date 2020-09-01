using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForMessageBusAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( MessageBusAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IMessageBus );

		public ExtensionForMessageBusAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.InstantiatePerContainer( typeof( IMessageBus ), typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
