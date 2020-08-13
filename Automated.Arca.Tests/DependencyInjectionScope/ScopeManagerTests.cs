using System.Reflection;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Tests.Dummies;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ScopeManagerTests
	{
		private const string ScopeName1 = "Tests.Scope1";
		private const string ScopeName2 = "Tests.Scope2";

		[Fact]
		public void InstantiatePerContainer_GetSameInstanceForGlobalScope()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, false );

			var instance1 = applicationPipeline.GetRequiredInstance<SomeInstantiatePerContainerComponent>();
			var instance2 = applicationPipeline.GetRequiredInstance<SomeInstantiatePerContainerComponent>();

			Assert.Equal( instance1, instance2 );
		}

		[Fact]
		public void InstantiatePerScope_GetSameInstanceForGlobalScope()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, true );

			var instance1 = applicationPipeline.GetRequiredInstance<ISomeTenantRequestProcessor>();
			var instance2 = applicationPipeline.GetRequiredInstance<ISomeTenantRequestProcessor>();

			Assert.Equal( instance1, instance2 );
			Assert.Equal( "", instance1.ScopeName );
			Assert.Equal( "", instance2.ScopeName );
			Assert.Equal( instance1.Level1, instance2.Level1 );
			Assert.Equal( instance1.Level1.Level2, instance2.Level1.Level2 );
		}

		[Fact]
		public void InstantiatePerScope_GetSameInstanceForSameScopeName()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, false );

			var scopedProvider1 = applicationPipeline.GetOrAddScopedProvider( ScopeName1 );
			var scopedProvider2 = applicationPipeline.GetOrAddScopedProvider( ScopeName1 );

			var tenantNameProvider1 = scopedProvider1.GetRequiredInstance<ITenantNameProvider>();
			var tenantNameProvider2 = scopedProvider2.GetRequiredInstance<ITenantNameProvider>();
			Assert.Equal( tenantNameProvider1, tenantNameProvider2 );

			SimulateTenantResolution( scopedProvider1, scopedProvider2 );

			var instance1 = scopedProvider1.GetRequiredInstance<ISomeTenantRequestProcessor>();
			var instance2 = scopedProvider2.GetRequiredInstance<ISomeTenantRequestProcessor>();

			Assert.Equal( instance1, instance2 );
			Assert.Equal( ScopeName1, instance1.ScopeName );
			Assert.Equal( ScopeName1, instance2.ScopeName );
			Assert.Equal( instance1.Level1, instance2.Level1 );
			Assert.Equal( instance1.Level1.Level2, instance2.Level1.Level2 );
		}

		[Fact]
		public void InstantiatePerScope_GetDifferentInstancesForDifferentScopes()
		{
			// This test proves that the (automatic) instantiation of the parameters of a class happens from the scope from which
			// the class was (manually) instantiated. This allows the developer to manually create a tenant-scoped processor which
			// has all its constructor parameters automatically instantiated, regardless of the level of imbrication. A request /
			// message handling method can then be called on the tenant-scoped processor.

			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, false );

			var scopedProvider1 = applicationPipeline.GetOrAddScopedProvider( ScopeName1 );
			var scopedProvider2 = applicationPipeline.GetOrAddScopedProvider( ScopeName2 );

			var tenantNameProvider1 = scopedProvider1.GetRequiredInstance<ITenantNameProvider>();
			var tenantNameProvider2 = scopedProvider2.GetRequiredInstance<ITenantNameProvider>();
			Assert.NotEqual( tenantNameProvider1, tenantNameProvider2 );

			SimulateTenantResolution( scopedProvider1, scopedProvider2 );

			var instance1 = scopedProvider1.GetRequiredInstance<ISomeTenantRequestProcessor>();
			var instance2 = scopedProvider2.GetRequiredInstance<ISomeTenantRequestProcessor>();

			Assert.NotEqual( instance1, instance2 );
			Assert.Equal( ScopeName1, instance1.ScopeName );
			Assert.Equal( ScopeName2, instance2.ScopeName );
			Assert.NotEqual( instance1.Level1, instance2.Level1 );
			Assert.NotEqual( instance1.Level1.Level2, instance2.Level1.Level2 );

			instance1.HandleRequest( new object() );
			instance2.HandleRequest( new object() );
		}

		[Fact]
		public void TenantScopeUsage()
		{
			InstantiatePerScope_GetDifferentInstancesForDifferentScopes();
		}

		private void SimulateTenantResolution( params IScopedInstanceProvider<string>[] scopedProviders )
		{
			foreach( var scopedProvider in scopedProviders )
			{
				var tenantNameProvider = scopedProvider.GetRequiredInstance<ITenantNameProvider>();

				tenantNameProvider.ScopeName = scopedProvider.ScopeName;
			}
		}
	}
}
