using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;
using Automated.Arca.Manager;

namespace Automated.Arca.Demo.WebApi.Components
{
	[InstantiatePerScopeAttribute]
	public class LogsProvider : IProcessable
	{
		private readonly CollectorLogger ManagerLogger;

		public LogsProvider( CollectorLogger managerLogger )
		{
			ManagerLogger = managerLogger;
		}

		public string Get()
		{
			return ManagerLogger.GetLogs();
		}
	}
}
