using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForCommandHandlerAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( CommandHandlerAttribute );

		public ExtensionForCommandHandlerAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.InstantiatePerScope( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var commandHandlerRegistry = D.P.GetRequiredInstance<ICommandHandlerRegistry>();

			commandHandlerRegistry.Add( typeWithAttribute );
		}
	}
}
