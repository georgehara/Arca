using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public abstract class ExtensionForChainMiddlewareAttribute : ExtensionForSpecializedAttribute
	{
		public ExtensionForChainMiddlewareAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			S.M.Chain( typeWithAttribute );
		}
	}
}
