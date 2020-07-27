using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.Cqrs;
using Automated.Arca.Libraries;

namespace Automated.Arca.Extensions.Cqrs
{
	public class ExtensionForDomainEventRegistryAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( DomainEventRegistryAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			typeWithAttribute.EnsureDerivesFrom( typeof( IDomainEventRegistry ) );

			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetDefaultInterface( typeWithAttribute );

			ToInstantiatePerContainer( context, interfaceType, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
