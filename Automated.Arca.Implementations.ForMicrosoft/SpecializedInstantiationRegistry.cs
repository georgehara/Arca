using System;
using System.Net.Http;
using System.Reflection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class SpecializedInstantiationRegistry : IInstantiationRegistry
	{
		protected IServiceCollection Services { get; private set; }

		public SpecializedInstantiationRegistry( IServiceCollection services )
		{
			Services = services;
		}

		public void AddExternalService( Type serviceInterfaceType, Type serviceImplementationType, string baseAddress,
			int retryCount, int retryDelayMilliseconds, int circuitBreakerEventCount, int circuitBreakerDurationSeconds )
		{
			serviceImplementationType.EnsureDerivesFromInterface( serviceInterfaceType );

			Action<HttpClient> action = client =>
			{
				client.BaseAddress = new Uri( baseAddress );
				client.DefaultRequestHeaders.Add( "Accept", "application/json" );
			};

			MethodInfo? method = typeof( HttpClientFactoryServiceCollectionExtensions )
				.GetMethod( nameof( HttpClientFactoryServiceCollectionExtensions.AddHttpClient ), 2,
					new Type[] { typeof( IServiceCollection ), typeof( Action<HttpClient> ) } );

			MethodInfo generic = method!.MakeGenericMethod( serviceInterfaceType, serviceImplementationType );

			var httpClientBuilder = generic!.Invoke( null, new object[] { Services, action } ) as IHttpClientBuilder;

			httpClientBuilder
				.AddTransientHttpErrorPolicy(
					p => p.WaitAndRetryAsync( retryCount, _ => TimeSpan.FromMilliseconds( retryDelayMilliseconds ) ) )
				.AddTransientHttpErrorPolicy(
					p => p.CircuitBreakerAsync( circuitBreakerEventCount, TimeSpan.FromSeconds( circuitBreakerDurationSeconds ) ) );

			var healthUri = $"{baseAddress}/health";
			var serviceName = $"External Service: {serviceImplementationType.Name} ({healthUri})";
			var tags = new[] { "externalService" };

			Services.AddHealthChecks().AddUrlGroup( new Uri( healthUri ), serviceName, tags: tags );
		}

		public void AddHostedService( Type hostedServiceType )
		{
			hostedServiceType.EnsureDerivesFromInterface( typeof( IHostedService ) );

			MethodInfo? method = typeof( ServiceCollectionHostedServiceExtensions )
				.GetMethod( nameof( ServiceCollectionHostedServiceExtensions.AddHostedService ), 1,
					new Type[] { typeof( IServiceCollection ) } );

			MethodInfo generic = method!.MakeGenericMethod( hostedServiceType );

			generic.Invoke( null, new object[] { Services } );
		}

		public void AddDatabaseContext( Type dbContextType, string connectionString, string migrationsHistoryTable,
			string migrationsHistoryTableSchema, int retryCount, int retryDelaySeconds )
		{
			dbContextType.EnsureDerivesFromNotEqual( typeof( DbContext ) );

			Action<DbContextOptionsBuilder> action = ob =>
			{
				ob.UseSqlServer(
					connectionString,
					sqlServerOptionsAction: sqlOptions =>
					{
						sqlOptions.EnableRetryOnFailure(
							maxRetryCount: retryCount,
							maxRetryDelay: TimeSpan.FromSeconds( retryDelaySeconds ),
							errorNumbersToAdd: null );

						sqlOptions.MigrationsHistoryTable( migrationsHistoryTable, migrationsHistoryTableSchema );
					}
				);
			};

			MethodInfo? method = typeof( EntityFrameworkServiceCollectionExtensions )
				.GetMethod( nameof( EntityFrameworkServiceCollectionExtensions.AddDbContext ), 1,
					new Type[] { typeof( IServiceCollection ), typeof( Action<DbContextOptionsBuilder> ),
						typeof( ServiceLifetime ), typeof( ServiceLifetime ) } );

			MethodInfo generic = method!.MakeGenericMethod( dbContextType );

			generic.Invoke( null, new object?[] { Services, action, null, null } );
		}
	}
}
