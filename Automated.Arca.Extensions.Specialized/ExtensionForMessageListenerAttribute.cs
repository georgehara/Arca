﻿using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForMessageListenerAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( MessageListenerAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IMessageListener<> );

		public ExtensionForMessageListenerAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.InstantiatePerInjection( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
