using Automated.Arca.Implementations.ForMicrosoft;
using Automated.Arca.Tests.Dummies;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Automated.Arca.Tests
{
	public class InstantiationRegistryTests
	{
		private readonly SpecializedInstantiationRegistry Registry;

		public InstantiationRegistryTests()
		{
			Registry = new SpecializedInstantiationRegistry( new ServiceCollection() );
		}

		[Fact]
		public void AddingExternalService_Succeeds()
		{
			Registry.AddExternalService( typeof( ISomeExternalService ), typeof( SomeExternalService ), "http://localhost",
				3, 500, 5, 30 );
		}

		[Fact]
		public void AddingHostedService_Succeeds()
		{
			Registry.AddHostedService( typeof( SomeHostedService ) );
		}

		[Fact]
		public void AddingBoundedContext_Succeeds()
		{
			Registry.AddDatabaseContext( typeof( SomeBoundedContext ), "SomeDatabaseContext", "MigrationsHistory",
				"MigrationsHistorySchema", 3, 30 );
		}
	}
}
