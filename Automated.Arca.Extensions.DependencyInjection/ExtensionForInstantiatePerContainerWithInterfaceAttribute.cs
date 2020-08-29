using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForInstantiatePerContainerWithInterfaceAttribute : ExtensionForDependencyInjectionAttribute
	{
		public override Type AttributeType => typeof( InstantiatePerContainerWithInterfaceAttribute );

		public ExtensionForInstantiatePerContainerWithInterfaceAttribute( IExtensionDependencyProvider extensionDependencyProvider )
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
