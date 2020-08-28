using Automated.Arca.Abstractions.Core;
using Automated.Arca.Extensions.DependencyInjection;
using Automated.Arca.Extensions.Specialized;
using Automated.Arca.Implementations.ForMicrosoft;
using Automated.Arca.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Automated.Arca.Demo.WebApi
{
	public class Startup
	{
		private readonly IConfiguration ApplicationOptionsProvider;
		private readonly IManagerLogger ManagerLogger;
		private readonly IManager Manager;

		public Startup( IConfiguration options )
		{
			ApplicationOptionsProvider = options;

			ManagerLogger = new CollectorLogger();

			var managerOptions = new ManagerOptions()
				.UseLogger( ManagerLogger )
				.UseOnlyClassesDerivedFromIProcessable();

			Manager = new Manager.Manager( managerOptions )
				.AddEntryAssembly()
				.AddAssemblyContainingType( typeof( ExtensionForInstantiatePerContainerAttribute ) )
				.AddAssemblyContainingType( typeof( ExtensionForBoundedContextAttribute ) )
				.AddKeyedOptionsProvider( ApplicationOptionsProvider );
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services )
		{
			services.AddControllers();

			services.AddSingleton( typeof( CollectorLogger ), ManagerLogger );

			Manager
				.AddDependencies( services )
				.Register();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
		{
			if( env.IsDevelopment() )
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			// When using middleware, "Manager.Configure" must be called before "app.UseEndpoints"!
			Manager
				.AddGlobalInstanceProvider( app.ApplicationServices )
				.AddMiddlewareRegistry( app )
				.Configure();

			app.UseEndpoints( endpoints => endpoints.MapControllers() );
		}
	}
}
