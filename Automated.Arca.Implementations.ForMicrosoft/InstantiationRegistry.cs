﻿using System;
using System.Net.Http;
using System.Reflection;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class InstantiationRegistry : IInstantiationRegistry
	{
		protected IServiceCollection Services { get; private set; }
		protected bool InstantiatePerContainerInsteadOfScope { get; private set; }

		public InstantiationRegistry( IServiceCollection services, bool instantiatePerContainerInsteadOfScope,
			bool addGlobalInstanceProvider )
		{
			Services = services;

			InstantiatePerContainerInsteadOfScope = instantiatePerContainerInsteadOfScope;

			if( addGlobalInstanceProvider )
				AddGlobalInstanceProvider();
		}

		public void ToInstantiatePerContainer( Type type )
		{
			Services.AddSingleton( type );
		}

		public void ToInstantiatePerContainer( Type baseType, Type implementationType )
		{
			Services.AddSingleton( baseType, implementationType );
		}

		public void ToInstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory )
		{
			Services.AddSingleton( baseType, implementationFactory );
		}

		public void ToInstantiatePerScope( Type type )
		{
			if( InstantiatePerContainerInsteadOfScope )
				ToInstantiatePerContainer( type );
			else
				Services.AddScoped( type );
		}

		public void ToInstantiatePerScope( Type baseType, Type implementationType )
		{
			if( InstantiatePerContainerInsteadOfScope )
				ToInstantiatePerContainer( baseType, implementationType );
			else
				Services.AddScoped( baseType, implementationType );
		}

		public void ToInstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory )
		{
			if( InstantiatePerContainerInsteadOfScope )
				ToInstantiatePerContainer( baseType, implementationFactory );
			else
				Services.AddScoped( baseType, implementationFactory );
		}

		public void ToInstantiatePerInjection( Type type )
		{
			Services.AddTransient( type );
		}

		public void ToInstantiatePerInjection( Type baseType, Type implementationType )
		{
			Services.AddTransient( baseType, implementationType );
		}

		public void ToInstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory )
		{
			Services.AddTransient( baseType, implementationFactory );
		}

		public void AddInstancePerContainer( Type baseType, object implementationInstance )
		{
			Services.AddSingleton( baseType, implementationInstance );
		}

		public void AddInstancePerContainer<T>( T instance )
			 where T : notnull
		{
			AddInstancePerContainer( typeof( T ), instance );
		}

		public void AddGlobalInstanceProvider()
		{
			ToInstantiatePerContainer( typeof( IGlobalInstanceProvider ),
				serviceProvider => new GlobalInstanceProvider( serviceProvider ) );

			ToInstantiatePerContainer( typeof( IInstanceProvider ),
				serviceProvider => serviceProvider.GetRequiredService<IGlobalInstanceProvider>() );
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
			dbContextType.EnsureDerivesFrom( typeof( DbContext ) );

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
