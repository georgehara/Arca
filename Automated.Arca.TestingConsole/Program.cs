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

			// These tests must be run separately because .Net caches (reflection) data in the process.
			t.ManagerPerformanceWithoutRegistrationAndConfigurationAndWithIProcessable();
			//t.ManagerPerformanceWithoutRegistrationAndConfigurationAndWithoutIProcessable();
		}
	}
}
