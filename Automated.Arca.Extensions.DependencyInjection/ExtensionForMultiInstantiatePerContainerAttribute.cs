using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForMultiInstantiatePerContainerAttribute : ExtensionForDependencyInjectionAttribute
	{
		public override Type AttributeType => typeof( MultiInstantiatePerContainerAttribute );

		public ExtensionForMultiInstantiatePerContainerAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (MultiInstantiateAttribute)attribute;
			var interfaceType = attributeTyped.GetInterfaceOrDefault( typeWithAttribute );

			D.M.Add( interfaceType, attributeTyped.ImplementationKey, typeWithAttribute );

			D.R.InstantiatePerContainer( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
