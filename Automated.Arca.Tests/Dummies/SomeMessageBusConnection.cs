using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeMessageBusConnection
	{
		string Connection { get; }
	}

	public class SomeMessageBusConnection : ISomeMessageBusConnection
	{
		public ILogger Logger { get; }
		public string Connection { get; }

		public SomeMessageBusConnection( ILogger logger, string connection )
		{
			Logger = logger;
			Connection = connection;
		}
	}

	public class SomeMessageBusConnectionRegistrator : DependencyInjectionRegistratorConfigurator
	{
		public SomeMessageBusConnectionRegistrator( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context )
		{
			D.R.ToInstantiatePerContainer( typeof( ISomeMessageBusConnection ),
				sp =>
				{
					var logger = sp.GetRequiredService<ILogger<SomeMessageBusConnection>>();

					return new SomeMessageBusConnection( logger, "Some connection string" );
				},
				false );
		}

		public override void Configure( IConfigurationContext context )
		{
		}
	}
}
