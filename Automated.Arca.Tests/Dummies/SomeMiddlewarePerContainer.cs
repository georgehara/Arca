using System.Threading.Tasks;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Automated.Arca.Tests
{
	[ChainMiddlewarePerContainerAttribute]
	public class SomeMiddlewarePerContainer : IMiddleware, IProcessable
	{
		private readonly ILogger<SomeMiddlewarePerContainer> Logger;

		public SomeMiddlewarePerContainer( ILogger<SomeMiddlewarePerContainer> logger )
		{
			Logger = logger;
		}

		public async Task InvokeAsync( HttpContext httpContext, RequestDelegate nextMiddleware )
		{
			DoStuff( httpContext );

			await nextMiddleware( httpContext );
		}

		private void DoStuff( HttpContext httpContext )
		{
			Logger.LogInformation( httpContext.TraceIdentifier );
		}
	}
}
