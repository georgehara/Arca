using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public static class ManagerExtensions
	{
		public static IManager AddKeyedOptionsProvider( this IManager manager, IConfiguration optionsProvider )
		{
			return manager
				.AddExtensionDependency<IKeyedOptionsProvider>( new KeyedOptionsProvider( optionsProvider ) );
		}

		public static IManager AddInstantiationRegistry( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope, bool addGlobalInstanceProvider )
		{
			var instantiationRegistry = new InstantiationRegistry( services, instantiatePerContainerInsteadOfScope,
				addGlobalInstanceProvider );

			return manager.AddExtensionDependency<IInstantiationRegistry>( instantiationRegistry );
		}

		public static IManager AddGlobalInstanceProvider( this IManager manager, IServiceProvider serviceProvider )
		{
			var globalInstanceProvider = serviceProvider.GetRequiredService<IGlobalInstanceProvider>();

			return manager
				.AddExtensionDependency<IGlobalInstanceProvider>( globalInstanceProvider )
				.AddExtensionDependency<IInstanceProvider>( globalInstanceProvider );
		}
	}
}
