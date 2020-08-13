using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForCommandHandlerAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( CommandHandlerAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ToInstantiatePerScope( context, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var commandHandlerRegistry = Provider( context ).GetRequiredInstance<ICommandHandlerRegistry>();

			commandHandlerRegistry.Add( typeWithAttribute );
		}
	}
}
