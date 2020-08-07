using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForInstantiatePerInjectionWithInterfaceAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( InstantiatePerInjectionWithInterfaceAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			ToInstantiatePerInjection( context, interfaceType, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
