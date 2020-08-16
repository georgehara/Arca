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
		public void ManagerPerformanceWithoutRegisterAndConfigure()
		{
			var statistics = GetManagerPerformance( false, true );

			DisplayTrace( nameof( ManagerPerformanceWithoutRegisterAndConfigure ), statistics );
		}

		[Fact]
		public void ManagerPerformanceWithIProcessable()
		{
			var statistics = GetManagerPerformance( true, true );

			DisplayTrace( nameof( ManagerPerformanceWithIProcessable ), statistics );
		}

		[Fact]
		public void ManagerPerformanceWithoutIProcessable()
		{
			var statistics = GetManagerPerformance( true, false );

			DisplayTrace( nameof( ManagerPerformanceWithoutIProcessable ), statistics );
		}

		private IManagerStatistics GetManagerPerformance( bool doRegisterAndConfigure, bool processOnlyTypesDerivedFromIProcessable )
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

			if( doRegisterAndConfigure )
			{
				applicationPipeline = new ApplicationPipeline( a, false, processOnlyTypesDerivedFromIProcessable, null, null,
					false, Assembly.GetExecutingAssembly(),
					x => x.AddAssembliesLoadedInProcess(),
					x => x.RegisterFirst(),
					x => x.ConfigureFirst() );
			}
			else
			{
				applicationPipeline = new ApplicationPipeline( a, false, processOnlyTypesDerivedFromIProcessable, null, null,
					false, Assembly.GetExecutingAssembly(),
					x => x.AddAssembliesLoadedInProcess(),
					x => { },
					x => { } );
			}

			return applicationPipeline.Statistics;
		}

		private void DisplayTrace( string methodName, IManagerStatistics statistics )
		{
			Trace.WriteLine( $"Executed method '{methodName}':" +
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
