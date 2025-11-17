using System.Diagnostics;

namespace Automated.Arca.Tests
{
	public abstract class PerformanceTests
	{
		protected static string Speed( Stopwatch sw, int iterations )
		{
			sw.Stop();

			var elapsedMilliseconds = sw.ElapsedMilliseconds;
			var iterationsPerSecond = elapsedMilliseconds > 0 ? iterations * 1000.0 / elapsedMilliseconds : 0;

			return $"{(long)iterationsPerSecond} #/s ({elapsedMilliseconds} ms)";
		}
	}
}
