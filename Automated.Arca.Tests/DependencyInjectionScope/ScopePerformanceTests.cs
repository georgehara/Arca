using System.Diagnostics;
using System.Reflection;
using Automated.Arca.Abstractions.DependencyInjection;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ScopePerformanceTests : PerformanceTests
	{
		[Fact]
		public void CreateScope()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, false );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 7000000;
			IScopedInstanceProvider<string> scopedProvider;

			for( int i = 0; i < iterations; i++ )
				scopedProvider = managerTooling.GetOrAddScopedProvider( ScopeNames.Main );

			Trace.WriteLine( $"Speed of '{nameof( CreateScope )}' = {Speed( sw, iterations )}" );
		}
	}
}
