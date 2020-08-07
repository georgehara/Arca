using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;
using Automated.Arca.Libraries;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForInstantiatePerContainerWithInterfaceAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( InstantiatePerContainerWithInterfaceAttribute );
		public virtual Type? RootInterfaceOfTypeWithAttribute => null;

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			if( RootInterfaceOfTypeWithAttribute != null )
				typeWithAttribute.EnsureDerivesFromGenericInterfaceWithSingleParameter( RootInterfaceOfTypeWithAttribute );

			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			ToInstantiatePerContainer( context, interfaceType, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
