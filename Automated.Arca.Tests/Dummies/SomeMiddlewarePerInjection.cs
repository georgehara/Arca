using System.Threading.Tasks;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Automated.Arca.Tests
{
	[ChainMiddlewarePerInjectionAttribute]
	public class SomeMiddlewarePerInjection : IMiddleware, IProcessable
	{
		private readonly ILogger<SomeMiddlewarePerInjection> Logger;

		public SomeMiddlewarePerInjection( ILogger<SomeMiddlewarePerInjection> logger )
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
