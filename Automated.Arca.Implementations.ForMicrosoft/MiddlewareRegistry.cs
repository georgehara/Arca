using System;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Libraries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class MiddlewareRegistry : IMiddlewareRegistry
	{
		protected IApplicationBuilder ApplicationBuilder { get; private set; }

		public MiddlewareRegistry( IApplicationBuilder applicationBuilder )
		{
			ApplicationBuilder = applicationBuilder;
		}

		public void Chain( Type type )
		{
			type.EnsureDerivesFromInterface( typeof( IMiddleware ) );

			ApplicationBuilder.UseMiddleware( type );
		}
	}
}
