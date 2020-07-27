using System;
using System.Reflection;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Tests.Dummies;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ManagerTests
	{
		[Fact]
		public void Processing_Succeeds()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			VerifyDummies( managerTooling, false );
		}

		[Fact]
		public void ProcessingWithoutDummyAssembly_Fails()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			VerifyDummies( managerTooling, false );

			void a() => managerTooling.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssembly_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, false )
				.AddAssemblyContainingType( typeof( global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent ) )
				.Register()
				.Configure();

			VerifyDummies( managerTooling, true );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndConfigure_Fails()
		{
			static void a() => new ManagerTooling( Assembly.GetExecutingAssembly(), true, false )
				.Register()
				.AddAssemblyContainingType( typeof( global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent ) )
				.Configure();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndRegisterAndConfigure_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, false )
				.Register()
				.AddAssemblyContainingType( typeof( global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent ) )
				.Register()
				.Configure();

			VerifyDummies( managerTooling, true );
		}

		[Fact]
		public void ProcessingWithMultipleCallsToRegisterAndConfigure_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, false )
				.Register()
				.Configure()
				.Register()
				.Configure();

			VerifyDummies( managerTooling, false );
		}

		[Fact]
		public void ProcessingWithMultipleCallsToRegisterAndConfigureAndDummyAssembly_Fails()
		{
			// Microsoft's dependency injection container stops registering components once the service provider is built,
			// without throwing an exception, so trying to register new types after "Configure" was called is pointless.

			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, false )
				.Register()
				.Configure()
				.AddAssemblyContainingType( typeof( global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent ) )
				.Register()
				.Configure();

			VerifyDummies( managerTooling, false );

			void a() => managerTooling.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithoutRegister_Fails()
		{
			static void a() => new ManagerTooling( Assembly.GetCallingAssembly(), true, false )
				.Configure();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithWrongRootAssembly_Fails()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetCallingAssembly(),
				true, false );

			void a() => managerTooling.GetRequiredInstance<SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Succeeds()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				false, false );

			var x = managerTooling.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>();

			Assert.NotNull( x );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Fails()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, false );

			void a() => managerTooling.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>();

			Assert.Throws<InvalidOperationException>( a );
		}

		private void VerifyDummies( ManagerTooling managerTooling, bool includeDummyAssembly )
		{
			var scopedProvider = managerTooling.GetOrAddScopedProvider( ScopeNames.Main );

			scopedProvider.GetRequiredInstance<SomeAttributeWithInstantiatePerScopeAttribute>();

			managerTooling.GetRequiredInstance<SomeBoundedContext>();

			managerTooling.GetRequiredInstance<ICommandHandlerRegistry>();
			scopedProvider.GetRequiredInstance<SomeCommandHandler>();

			var someComponentForRegistratorConfigurator
				= scopedProvider.GetRequiredInstance<SomeComponentForRegistratorConfigurator>();
			Assert.True( someComponentForRegistratorConfigurator.Configured );

			// "SomeComponentNotDerivedFromIProcessable" is checked separately.

			var someComponentWithInterfaceSpecifiedInAttribute
				= managerTooling.GetRequiredInstance<ISomeComponentWithInterfaceSpecifiedInAttribute>();
			Assert.True( someComponentWithInterfaceSpecifiedInAttribute.Configured );

			var someComponentWithoutInterfaceSpecifiedInAttribute
				= managerTooling.GetRequiredInstance<ISomeComponentWithoutInterfaceSpecifiedInAttribute>();
			Assert.True( someComponentWithoutInterfaceSpecifiedInAttribute.Configured );

			managerTooling.GetRequiredInstance<IDomainEventRegistry>();
			managerTooling.GetRequiredInstance<IDomainEventHandlerRegistry>();
			scopedProvider.GetRequiredInstance<SomeDomainEventHandler>();

			managerTooling.GetRequiredInstance<ISomeExternalService>();

			scopedProvider.GetRequiredInstance<SomeInstantiatePerScopeComponent>();
			scopedProvider.GetRequiredInstance<ISomeInstantiatePerScopeComponentWithInterface>();
			managerTooling.GetRequiredInstance<SomeInstantiatePerInjectionComponent>();
			managerTooling.GetRequiredInstance<ISomeInstantiatePerInjectionComponentWithInterface>();
			managerTooling.GetRequiredInstance<SomeInstantiatePerContainerComponent>();
			managerTooling.GetRequiredInstance<ISomeInstantiatePerContainerComponentWithInterface>();

			managerTooling.GetRequiredInstance<IIntegrationEventHandlerRegistry>();
			scopedProvider.GetRequiredInstance<SomeIntegrationEventHandler>();

			var someMessageBusConnection = managerTooling.GetRequiredInstance<ISomeMessageBusConnection>();
			Assert.NotNull( someMessageBusConnection.Connection );

			scopedProvider.GetRequiredInstance<SomeOutbox>();
			managerTooling.GetRequiredInstance<IOutboxProcessor>();

			scopedProvider.GetRequiredInstance<SomeProcessableAttributeWithInstantiatePerScopeAttribute>();

			scopedProvider.GetRequiredInstance<SomeScopedComponent>();

			if( includeDummyAssembly )
				VerifyDummiesFromDummyAssembly( managerTooling );
		}

		private void VerifyDummiesFromDummyAssembly( ManagerTooling managerTooling )
		{
			var scopedProvider = managerTooling.GetOrAddScopedProvider( ScopeNames.Main );

			scopedProvider.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();
		}
	}
}
