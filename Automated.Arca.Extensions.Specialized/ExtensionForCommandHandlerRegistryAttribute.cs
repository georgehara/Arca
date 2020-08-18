using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;
using Automated.Arca.Libraries;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForCommandHandlerRegistryAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( CommandHandlerRegistryAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			typeWithAttribute.EnsureDerivesFrom( typeof( ICommandHandlerRegistry ) );

			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			ToInstantiatePerContainer( context, interfaceType, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
