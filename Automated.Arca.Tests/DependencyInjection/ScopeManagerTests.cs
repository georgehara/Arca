using System.Reflection;
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

			var instance1 = managerTooling.GetRequiredInstance<SomeScopedComponent>();
			var instance2 = managerTooling.GetRequiredInstance<SomeScopedComponent>();

			Assert.Equal( instance1, instance2 );
			Assert.Equal( instance1.OtherComponent, instance2.OtherComponent );
		}

		[Fact]
		public void InstantiatePerScope_GetSameInstanceForSameScopeName()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			var scopedProvider1 = managerTooling.GetOrAddScopedProvider( ScopeName1 );
			var scopedProvider2 = managerTooling.GetOrAddScopedProvider( ScopeName1 );

			var instance1 = scopedProvider1.GetRequiredInstance<SomeScopedComponent>();
			var instance2 = scopedProvider2.GetRequiredInstance<SomeScopedComponent>();

			Assert.Equal( instance1, instance2 );
			Assert.Equal( instance1.OtherComponent, instance2.OtherComponent );
		}

		[Fact]
		public void InstantiatePerScope_GetDifferentInstancesForDifferentScopes()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			var scopedProvider1 = managerTooling.GetOrAddScopedProvider( ScopeName1 );
			var scopedProvider2 = managerTooling.GetOrAddScopedProvider( ScopeName2 );

			var instance1 = scopedProvider1.GetRequiredInstance<SomeScopedComponent>();
			var instance2 = scopedProvider2.GetRequiredInstance<SomeScopedComponent>();

			Assert.NotEqual( instance1, instance2 );
			Assert.NotEqual( instance1.OtherComponent, instance2.OtherComponent );
		}
	}
}
