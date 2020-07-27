using System.Diagnostics;

namespace Automated.Arca.Tests
{
	public abstract class PerformanceTests
	{
		protected string Speed( Stopwatch sw, int iterations )
		{
			sw.Stop();

			var elapsedMilliseconds = sw.ElapsedMilliseconds;
			var iterationsPerSecond = elapsedMilliseconds > 0 ? iterations * 1000.0 / elapsedMilliseconds : 0;

			return $"{(int)iterationsPerSecond} #/s ({elapsedMilliseconds} ms)";
		}
	}
}
