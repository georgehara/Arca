using System;
using System.Linq;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class InstantiationRegistry : IInstantiationRegistry
	{
		protected IManager Manager { get; private set; }
		protected IServiceCollection Services { get; private set; }
		protected bool InstantiatePerContainerInsteadOfScope { get; private set; }
		protected IAutomatedMocker? AutomatedMocker { get; private set; }

		protected bool IsManualMocking { get; private set; }

		public InstantiationRegistry( IManager manager, IServiceCollection services, bool instantiatePerContainerInsteadOfScope,
			IAutomatedMocker? automatedMocker )
		{
			Manager = manager;
			Services = services;
			InstantiatePerContainerInsteadOfScope = instantiatePerContainerInsteadOfScope;
			AutomatedMocker = automatedMocker;
		}

		public void InstantiatePerContainer( Type type, bool overrideExisting )
		{
			SetupInstantiate( type, overrideExisting );

			if( MustAvoidAutomatedMocking( type ) )
				Services.AddSingleton( type );
			else
				Services.AddSingleton( type, sp => GetMock( type ) );
		}

		public void InstantiatePerContainer( Type baseType, Type implementationType, bool overrideExisting )
		{
			SetupInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddSingleton( baseType, implementationType );
			else
				Services.AddSingleton( baseType, sp => GetMock( baseType ) );
		}

		public void InstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting )
		{
			SetupInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddSingleton( baseType, implementationFactory );
			else
				Services.AddSingleton( baseType, sp => GetMock( baseType ) );
		}

		public void InstantiatePerScope( Type type, bool overrideExisting )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				InstantiatePerContainer( type, overrideExisting );
			}
			else
			{
				SetupInstantiate( type, overrideExisting );

				if( MustAvoidAutomatedMocking( type ) )
					Services.AddScoped( type );
				else
					Services.AddScoped( type, sp => GetMock( type ) );
			}
		}

		public void InstantiatePerScope( Type baseType, Type implementationType, bool overrideExisting )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				InstantiatePerContainer( baseType, implementationType, overrideExisting );
			}
			else
			{
				SetupInstantiate( baseType, overrideExisting );

				if( MustAvoidAutomatedMocking( baseType ) )
					Services.AddScoped( baseType, implementationType );
				else
					Services.AddScoped( baseType, sp => GetMock( baseType ) );
			}
		}

		public void InstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				InstantiatePerContainer( baseType, implementationFactory, overrideExisting );
			}
			else
			{
				SetupInstantiate( baseType, overrideExisting );

				if( MustAvoidAutomatedMocking( baseType ) )
					Services.AddScoped( baseType, implementationFactory );
				else
					Services.AddScoped( baseType, sp => GetMock( baseType ) );
			}
		}

		public void InstantiatePerInjection( Type type, bool overrideExisting )
		{
			SetupInstantiate( type, overrideExisting );

			if( MustAvoidAutomatedMocking( type ) )
				Services.AddTransient( type );
			else
				Services.AddTransient( type, sp => GetMock( type ) );
		}

		public void InstantiatePerInjection( Type baseType, Type implementationType, bool overrideExisting )
		{
			SetupInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddTransient( baseType, implementationType );
			else
				Services.AddTransient( baseType, sp => GetMock( baseType ) );
		}

		public void InstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting )
		{
			SetupInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddTransient( baseType, implementationFactory );
			else
				Services.AddTransient( baseType, sp => GetMock( baseType ) );
		}

		public void AddInstancePerContainer( Type baseType, object implementationInstance, bool overrideExisting )
		{
			SetupInstantiate( baseType, overrideExisting );

			Services.AddSingleton( baseType, implementationInstance );
		}

		public void AddInstancePerContainer<T>( T instance, bool overrideExisting )
			 where T : notnull
		{
			AddInstancePerContainer( typeof( T ), instance, overrideExisting );
		}

		public IInstantiationRegistry ActivateManualMocking()
		{
			IsManualMocking = true;

			return this;
		}

		public IInstantiationRegistry WithManualMocking( ManualMocker manualMocker )
		{
			IsManualMocking = true;

			manualMocker( this );

			return this;
		}

		private void SetupInstantiate( Type type, bool overrideExisting )
		{
			if( overrideExisting )
			{
				var toRemove = Services.FirstOrDefault( d => d.ServiceType == type );

				if( toRemove != null )
					Services.Remove( toRemove );
			}
			else if( Services.Any( d => d.ServiceType == type ) )
			{
				throw new InvalidOperationException( $"Type '{type.Name}' was already registered for dependency injection." );
			}
		}

		private bool MustAvoidAutomatedMocking( Type type )
		{
			return
				AutomatedMocker == null ||
				IsManualMocking ||
				AutomatedMocker.MustAvoidMocking( Manager, type );
		}

		private object GetMock( Type type )
		{
			return AutomatedMocker!.GetMock( Manager, type );
		}
	}
}
