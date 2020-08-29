using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForMultiInstantiatePerScopeAttribute : ExtensionForDependencyInjectionAttribute
	{
		public override Type AttributeType => typeof( MultiInstantiatePerScopeAttribute );

		public ExtensionForMultiInstantiatePerScopeAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (MultiInstantiateAttribute)attribute;

			D.M.Add( attributeTyped.InterfaceType, attributeTyped.ImplementationKey, typeWithAttribute );

			D.R.InstantiatePerScope( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
