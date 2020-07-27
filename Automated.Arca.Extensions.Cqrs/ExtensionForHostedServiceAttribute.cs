using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Extensions.Cqrs
{
	public class ExtensionForHostedServiceAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( HostedServiceAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			Registry( context ).AddHostedService( typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
