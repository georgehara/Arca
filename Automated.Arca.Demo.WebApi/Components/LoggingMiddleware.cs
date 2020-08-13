using System.Threading.Tasks;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;
using Automated.Arca.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Automated.Arca.Demo.WebApi.Components
{
	[ChainMiddlewarePerScopeAttribute]
	public class LoggingMiddleware : IMiddleware, IProcessable
	{
		private readonly CollectorLogger ManagerLogger;

		public LoggingMiddleware( CollectorLogger managerLogger )
		{
			ManagerLogger = managerLogger;
		}

		public async Task InvokeAsync( HttpContext httpContext, RequestDelegate nextMiddleware )
		{
			Log( httpContext );

			await nextMiddleware( httpContext );
		}

		private void Log( HttpContext httpContext )
		{
			ManagerLogger.Log( $"Invoking middleware '{nameof( LoggingMiddleware )}':" );
			ManagerLogger.Log( $"\t* URL: {httpContext.Request.GetDisplayUrl()}" );
			ManagerLogger.Log( $"\t* Endpoint: {httpContext.GetEndpoint().DisplayName}" );
			ManagerLogger.Log( $"\t* Request identifier: {httpContext.TraceIdentifier}" );
		}
	}
}
