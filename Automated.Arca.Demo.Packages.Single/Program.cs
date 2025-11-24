using System;
using System.IO;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Extensions.DependencyInjection;
using Automated.Arca.Extensions.Specialized;
using Automated.Arca.Implementations.ForMicrosoft;
using Automated.Arca.Manager;
using Microsoft.Extensions.Configuration;

namespace Automated.Arca.Demo.Packages.Single
{
	internal class Program
	{
		static void Main( string[] args )
		{
			Process();

			Console.WriteLine( "The inclusion of the 'Single' package was successful." );
		}

		private static void Process()
		{
			var applicationOptionsProvider = GetApplicationOptionsProvider();
			var logger = new TraceLogger();
			var managerOptions = GetManagerOptions( logger );

			GetManager( managerOptions, false, applicationOptionsProvider, Assembly.GetExecutingAssembly() );
		}

		private static IConfiguration GetApplicationOptionsProvider()
		{
			return new ConfigurationBuilder()
				.SetBasePath( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location )! )
				.AddJsonFile( "appsettings.json", optional: false, reloadOnChange: false )
				.AddEnvironmentVariables()
				.Build();
		}

		private static IManagerOptions GetManagerOptions( IManagerLogger logger )
		{
			return new ManagerOptions()
				.UseLogger( logger )
				.UseOnlyClassesDerivedFromIProcessable();
		}

		private static IManager GetManager( IManagerOptions managerOptions, bool simulateOnlyUnprocessableTypes,
			IConfiguration applicationOptionsProvider, Assembly rootAssembly )
		{
			return new Manager.Manager( managerOptions, simulateOnlyUnprocessableTypes )
				.AddAssembly( rootAssembly )
				.AddAssemblyContainingType<ExtensionForInstantiatePerScopeAttribute>()
				.AddAssemblyContainingType<ExtensionForBoundedContextAttribute>()
				.AddKeyedOptionsProvider( applicationOptionsProvider );
		}
	}
}
