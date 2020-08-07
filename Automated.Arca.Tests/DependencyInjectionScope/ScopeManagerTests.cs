using System.Reflection;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Implementations.ForMicrosoft;
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
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			var instance1 = managerTooling.GetRequiredInstance<SomeInstantiatePerContainerComponent>();
			var instance2 = managerTooling.GetRequiredInstance<SomeInstantiatePerContainerComponent>();

			Assert.Equal( instance1, instance2 );
		}

		[Fact]
		public void InstantiatePerScope_GetSameInstanceForGlobalScope()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, true );

			var instance1 = managerTooling.GetRequiredInstance<ISomeTenantRequestProcessor>();
			var instance2 = managerTooling.GetRequiredInstance<ISomeTenantRequestProcessor>();

			Assert.Equal( instance1, instance2 );
			Assert.Equal( "", instance1.ScopeName );
			Assert.Equal( "", instance2.ScopeName );
			Assert.Equal( instance1.Level1, instance2.Level1 );
			Assert.Equal( instance1.Level1.Level2, instance2.Level1.Level2 );
		}

		[Fact]
		public void InstantiatePerScope_GetSameInstanceForSameScopeName()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			var scopedProvider1 = managerTooling.GetOrAddScopedProvider( ScopeName1 );
			var scopedProvider2 = managerTooling.GetOrAddScopedProvider( ScopeName1 );

			Assert.Equal( ((InstanceProvider)scopedProvider1).Dependency, ((InstanceProvider)scopedProvider2).Dependency );

			var tenantHolder1 = scopedProvider1.GetRequiredInstance<ITenantHolder>();
			var tenantHolder2 = scopedProvider2.GetRequiredInstance<ITenantHolder>();
			Assert.Equal( tenantHolder1, tenantHolder2 );

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
			// has all its constructor parameters automatically instantiated. A request / message handling method can then be
			// called on the tenant-scoped processor.

			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			var scopedProvider1 = managerTooling.GetOrAddScopedProvider( ScopeName1 );
			var scopedProvider2 = managerTooling.GetOrAddScopedProvider( ScopeName2 );

			Assert.NotEqual( ((InstanceProvider)scopedProvider1).Dependency, ((InstanceProvider)scopedProvider2).Dependency );

			var tenantHolder1 = scopedProvider1.GetRequiredInstance<ITenantHolder>();
			var tenantHolder2 = scopedProvider2.GetRequiredInstance<ITenantHolder>();
			Assert.NotEqual( tenantHolder1, tenantHolder2 );

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
				var tenantHolder = scopedProvider.GetRequiredInstance<ITenantHolder>();

				tenantHolder.ScopeName = scopedProvider.ScopeName;
			}
		}
	}
}
