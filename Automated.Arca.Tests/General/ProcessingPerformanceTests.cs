using System.Diagnostics;
using System.Reflection;
using Automated.Arca.Manager;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ProcessingPerformanceTests : PerformanceTests
	{
		[Fact]
		public void ManagerPerformanceWithIProcessable()
		{
			var elapsedMilliseconds = ManagerPerformance( true );

			Trace.WriteLine( $"Method '{nameof( ManagerPerformanceWithIProcessable )}' executed in {elapsedMilliseconds} ms." );
		}

		[Fact]
		public void ManagerPerformanceWithoutIProcessable()
		{
			var elapsedMilliseconds = ManagerPerformance( false );

			Trace.WriteLine( $"Method '{nameof( ManagerPerformanceWithoutIProcessable )}' executed in {elapsedMilliseconds} ms." );
		}

		private long ManagerPerformance( bool processOnlyTypesDerivedFromIProcessable )
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			static void a( IManagerOptions x ) => x
				.AddAssemblyNamePrefix( "" )
				.ExcludeAssemblyName( "System.Data.OleDb" ) // Exclude missing assemblies.
				.ExcludeAssemblyName( "Windows" )
				.ExcludeAssemblyName( "Windows.Storage" )
				.ExcludeAssemblyName( "Windows.System" )
				.ExcludeAssemblyName( "Windows.Devices" )
				.ExcludeAssemblyName( "Windows.Data" )
				.ExcludeAssemblyName( "System.ServiceModel.Syndication" )
				.ExcludeAssemblyName( "System.ServiceProcess.ServiceController" )
				.ExcludeAssemblyName( "System.CodeDom" )
				.ExcludeAssemblyName( "System.IO.Ports" )
				.ExcludeAssemblyName( "System.IO.Packaging" )
				.ExcludeAssemblyName( "System.Threading.AccessControl" )
				.ExcludeAssemblyName( "System.Diagnostics.PerformanceCounter" )
				.ExcludeAssemblyName( "System.Data.SqlClient" )
				.ExcludeAssemblyName( "System.Data.Odbc" );

			new ApplicationPipeline( a, false, processOnlyTypesDerivedFromIProcessable, null, null, false,
				Assembly.GetExecutingAssembly(),
				x => { },
				x => x.AddAssembliesLoadedInProcess()
					.RegisterFirst(),
				x => x.ConfigureFirst() );

			return sw.ElapsedMilliseconds;
		}
	}
}
