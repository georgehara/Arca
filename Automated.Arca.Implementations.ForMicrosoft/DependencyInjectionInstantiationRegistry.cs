using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class DependencyInjectionInstantiationRegistry : IInstantiationRegistry
	{
		protected IServiceCollection Services { get; private set; }
		protected bool InstantiatePerContainerInsteadOfScope { get; private set; }

		public DependencyInjectionInstantiationRegistry( IServiceCollection services, bool instantiatePerContainerInsteadOfScope,
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
	}
}
