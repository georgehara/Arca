using System;
using System.Linq;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class DependencyInjectionInstantiationRegistry : IInstantiationRegistry
	{
		protected IManager Manager { get; private set; }
		protected IServiceCollection Services { get; private set; }
		protected bool AllowMultipleImplementationsPerBaseType { get; private set; }
		protected bool InstantiatePerContainerInsteadOfScope { get; private set; }
		protected AutomatedMocker? AutomatedMocker { get; private set; }
		protected bool IsManualMocking { get; private set; }

		public DependencyInjectionInstantiationRegistry( IManager manager, IServiceCollection services,
			bool allowMultipleImplementationsPerBaseType, bool instantiatePerContainerInsteadOfScope,
			AutomatedMocker? automatedMocker, bool addGlobalInstanceProvider )
		{
			Manager = manager;
			Services = services;
			AllowMultipleImplementationsPerBaseType = allowMultipleImplementationsPerBaseType;
			InstantiatePerContainerInsteadOfScope = instantiatePerContainerInsteadOfScope;
			AutomatedMocker = automatedMocker;

			if( addGlobalInstanceProvider )
				AddGlobalInstanceProvider();
		}

		public void ToInstantiatePerContainer( Type type, bool overrideExisting = false )
		{
			PrepareOverrideExisting( type, overrideExisting );

			if( MustAvoidAutomatedMocking( type ) )
				Services.AddSingleton( type );
			else
				Services.AddSingleton( type, sp => GetMock( type ) );
		}

		public void ToInstantiatePerContainer( Type baseType, Type implementationType, bool overrideExisting = false )
		{
			PrepareOverrideExisting( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddSingleton( baseType, implementationType );
			else
				Services.AddSingleton( baseType, sp => GetMock( baseType ) );
		}

		public void ToInstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting = false )
		{
			PrepareOverrideExisting( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddSingleton( baseType, implementationFactory );
			else
				Services.AddSingleton( baseType, sp => GetMock( baseType ) );
		}

		public void ToInstantiatePerScope( Type type, bool overrideExisting = false )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				ToInstantiatePerContainer( type );
			}
			else
			{
				PrepareOverrideExisting( type, overrideExisting );

				if( MustAvoidAutomatedMocking( type ) )
					Services.AddScoped( type );
				else
					Services.AddScoped( type, sp => GetMock( type ) );
			}
		}

		public void ToInstantiatePerScope( Type baseType, Type implementationType, bool overrideExisting = false )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				ToInstantiatePerContainer( baseType, implementationType );
			}
			else
			{
				PrepareOverrideExisting( baseType, overrideExisting );

				if( MustAvoidAutomatedMocking( baseType ) )
					Services.AddScoped( baseType, implementationType );
				else
					Services.AddScoped( baseType, sp => GetMock( baseType ) );
			}
		}

		public void ToInstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting = false )
		{
			if( InstantiatePerContainerInsteadOfScope )
			{
				ToInstantiatePerContainer( baseType, implementationFactory );
			}
			else
			{
				PrepareOverrideExisting( baseType, overrideExisting );

				if( MustAvoidAutomatedMocking( baseType ) )
					Services.AddScoped( baseType, implementationFactory );
				else
					Services.AddScoped( baseType, sp => GetMock( baseType ) );
			}
		}

		public void ToInstantiatePerInjection( Type type, bool overrideExisting = false )
		{
			PrepareOverrideExisting( type, overrideExisting );

			if( MustAvoidAutomatedMocking( type ) )
				Services.AddTransient( type );
			else
				Services.AddTransient( type, sp => GetMock( type ) );
		}

		public void ToInstantiatePerInjection( Type baseType, Type implementationType, bool overrideExisting = false )
		{
			PrepareOverrideExisting( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddTransient( baseType, implementationType );
			else
				Services.AddTransient( baseType, sp => GetMock( baseType ) );
		}

		public void ToInstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory,
			bool overrideExisting = false )
		{
			PrepareOverrideExisting( baseType, overrideExisting );

			if( MustAvoidAutomatedMocking( baseType ) )
				Services.AddTransient( baseType, implementationFactory );
			else
				Services.AddTransient( baseType, sp => GetMock( baseType ) );
		}

		public void AddInstancePerContainer( Type baseType, object implementationInstance, bool overrideExisting = false )
		{
			PrepareOverrideExisting( baseType, overrideExisting );

			Services.AddSingleton( baseType, implementationInstance );
		}

		public void AddInstancePerContainer<T>( T instance, bool overrideExisting = false )
			 where T : notnull
		{
			AddInstancePerContainer( typeof( T ), instance, overrideExisting );
		}

		public void AddGlobalInstanceProvider()
		{
			ToInstantiatePerContainer( typeof( IGlobalInstanceProvider ),
				serviceProvider => new GlobalInstanceProvider( serviceProvider ) );

			ToInstantiatePerContainer( typeof( IInstanceProvider ),
				serviceProvider => serviceProvider.GetRequiredService<IGlobalInstanceProvider>() );
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

		private void PrepareOverrideExisting( Type type, bool overrideExisting )
		{
			if( overrideExisting )
			{
				var toRemove = Services.FirstOrDefault( d => d.ServiceType == type );

				if( toRemove != null )
					Services.Remove( toRemove );
			}
			else if( !AllowMultipleImplementationsPerBaseType && Services.Any( d => d.ServiceType == type ) )
			{
				throw new InvalidOperationException( $"Multiple dependency injection implementation registrations are not" +
					$" allowed for type '{type.Name}'." );
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
