using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public abstract class ExtensionForChainMiddlewareAttribute : ExtensionForProcessableAttribute
	{
		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ChainMiddleware( context, typeWithAttribute );
		}

		protected IMiddlewareRegistry MiddlewareRegistry( IConfigurationContext context )
		{
			return GetExtensionDependency<IMiddlewareRegistry>( context );
		}

		protected void ChainMiddleware( IConfigurationContext context, Type type )
		{
			MiddlewareRegistry( context ).Chain( type );
		}
	}
}
