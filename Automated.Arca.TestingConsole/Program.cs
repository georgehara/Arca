using System.Diagnostics;
using Automated.Arca.Tests;

namespace Automated.Arca.TestingConsole
{
	class Program
	{
		static void Main()
		{
			RedirectTraceOutputToConsole();

			ProcessingPerformanceTests();
		}

		private static void RedirectTraceOutputToConsole()
		{
			Trace.Listeners.Add( new ConsoleTraceListener() );
		}

		private static void ProcessingPerformanceTests()
		{
			var t = new ProcessingPerformanceTests();

			t.ManagerPerformanceWithoutRegistrationAndConfigurationAndWithIProcessable();
			//t.ManagerPerformanceWithoutRegistrationAndConfigurationAndWithoutIProcessable();
		}
	}
}
