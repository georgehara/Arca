using Automated.Arca.Demo.WebApi.Components;
using Microsoft.AspNetCore.Mvc;

namespace Automated.Arca.Demo.WebApi.Controllers
{
	[ApiController]
	[Route( "[controller]" )]
	public class LogsController : ControllerBase
	{
		private readonly LogsProvider LogsProvider;

		public LogsController( LogsProvider logsProvider )
		{
			LogsProvider = logsProvider;
		}

		[HttpGet]
		public object Get()
		{
			return LogsProvider.Get();
		}
	}
}
