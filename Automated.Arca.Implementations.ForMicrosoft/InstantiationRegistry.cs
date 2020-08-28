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

		public void ToInstantiatePerContainer( Type type, bool overrideExisting )
		{
			SetupToInstantiate( type, overrideExisting );

			if( MustAvoidAutomatedMocking( type ) )
				Services.AddSingleton( type );
			else
				Services.AddSingleton( type, sp => GetMock( type ) );
		}

		public void ToInstantiatePerContainer( Type baseType, Type implementationType, bool overrideExisting )
		{
			SetupToInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddSingleton( baseType, implementationType );
			else
				Services.AddSingleton( baseType, sp => GetMock( baseType ) );
		}

		public void ToInstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting )
		{
			SetupToInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddSingleton( baseType, implementationFactory );
			else
				Services.AddSingleton( baseType, sp => GetMock( baseType ) );
		}

		public void ToInstantiatePerScope( Type type, bool overrideExisting )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				ToInstantiatePerContainer( type, overrideExisting );
			}
			else
			{
				SetupToInstantiate( type, overrideExisting );

				if( MustAvoidAutomatedMocking( type ) )
					Services.AddScoped( type );
				else
					Services.AddScoped( type, sp => GetMock( type ) );
			}
		}

		public void ToInstantiatePerScope( Type baseType, Type implementationType, bool overrideExisting )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				ToInstantiatePerContainer( baseType, implementationType, overrideExisting );
			}
			else
			{
				SetupToInstantiate( baseType, overrideExisting );

				if( MustAvoidAutomatedMocking( baseType ) )
					Services.AddScoped( baseType, implementationType );
				else
					Services.AddScoped( baseType, sp => GetMock( baseType ) );
			}
		}

		public void ToInstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				ToInstantiatePerContainer( baseType, implementationFactory, overrideExisting );
			}
			else
			{
				SetupToInstantiate( baseType, overrideExisting );

				if( MustAvoidAutomatedMocking( baseType ) )
					Services.AddScoped( baseType, implementationFactory );
				else
					Services.AddScoped( baseType, sp => GetMock( baseType ) );
			}
		}

		public void ToInstantiatePerInjection( Type type, bool overrideExisting )
		{
			SetupToInstantiate( type, overrideExisting );

			if( MustAvoidAutomatedMocking( type ) )
				Services.AddTransient( type );
			else
				Services.AddTransient( type, sp => GetMock( type ) );
		}

		public void ToInstantiatePerInjection( Type baseType, Type implementationType, bool overrideExisting )
		{
			SetupToInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddTransient( baseType, implementationType );
			else
				Services.AddTransient( baseType, sp => GetMock( baseType ) );
		}

		public void ToInstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting )
		{
			SetupToInstantiate( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddTransient( baseType, implementationFactory );
			else
				Services.AddTransient( baseType, sp => GetMock( baseType ) );
		}

		public void AddInstancePerContainer( Type baseType, object implementationInstance, bool overrideExisting )
		{
			SetupToInstantiate( baseType, overrideExisting );

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

		private void SetupToInstantiate( Type type, bool overrideExisting )
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
