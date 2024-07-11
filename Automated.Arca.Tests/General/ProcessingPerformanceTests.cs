using System.Diagnostics;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Manager;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ProcessingPerformanceTests : PerformanceTests
	{
		[Fact]
		public void ManagerPerformanceWithoutRegistrationAndConfigurationAndWithIProcessable()
		{
			var statistics = GetManagerPerformance( true, true );

			DisplayTrace( "Performance with 'IProcessable'", statistics );
		}

		[Fact]
		public void ManagerPerformanceWithoutRegistrationAndConfigurationAndWithoutIProcessable()
		{
			var statistics = GetManagerPerformance( false, true );

			DisplayTrace( "Performance without 'IProcessable'", statistics );
		}

		private static IManagerStatistics GetManagerPerformance( bool processOnlyTypesDerivedFromIProcessable,
			bool simulateOnlyUnprocessableTypes )
		{
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

			ApplicationPipeline applicationPipeline;

			applicationPipeline = new ApplicationPipeline( a, false, processOnlyTypesDerivedFromIProcessable, null, null,
				simulateOnlyUnprocessableTypes, false, null, Assembly.GetExecutingAssembly(),
				x => x.AddAssembliesLoadedInProcess(),
				x => x.RegisterFirst(),
				x => x.ConfigureFirst() );

			return applicationPipeline.Statistics;
		}

		private static void DisplayTrace( string testDescription, IManagerStatistics statistics )
		{
			Trace.WriteLine( $"{testDescription}:" +
				$" Loaded {statistics.LoadedAssemblies} assemblies." +
				$" Assemblies loaded in {statistics.AssemblyLoadingTime} ms." +
				$" Data cached in {statistics.AssemblyLoadingTime} ms." +
				$" Cached {statistics.CachedTypes} types." +
				$" Registered {statistics.RegisteredClasses} classes." +
				$" Classes registered in {statistics.ClassRegistrationTime} ms." +
				$" Classes configured in {statistics.ClassConfigurationTime} ms." );
		}
	}
}
