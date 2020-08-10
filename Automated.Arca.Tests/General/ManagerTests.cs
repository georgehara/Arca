using System;
using System.Collections.Generic;
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
				true, null, false );

			VerifyDummies( managerTooling, false );
		}

		[Fact]
		public void ProcessingWithoutDummyAssembly_Fails()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, false );

			VerifyDummies( managerTooling, false );

			void a() => managerTooling.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssembly_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, null, false )
				.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
				.Register()
				.Configure();

			VerifyDummies( managerTooling, true );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndConfigure_Fails()
		{
			static void a() => new ManagerTooling( Assembly.GetExecutingAssembly(), true, null, false )
				.Register()
				.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
				.Configure();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndRegisterAndConfigure_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, null, false )
				.Register()
				.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
				.Register()
				.Configure();

			VerifyDummies( managerTooling, true );
		}

		[Fact]
		public void ProcessingWithMultipleCallsToRegisterAndConfigure_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, null, false )
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

			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), true, null, false )
				.Register()
				.Configure()
				.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
				.Register()
				.Configure();

			VerifyDummies( managerTooling, false );

			void a() => managerTooling.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithoutRegister_Fails()
		{
			static void a() => new ManagerTooling( Assembly.GetCallingAssembly(), true, null, false )
				.Configure();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithWrongRootAssembly_Fails()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetCallingAssembly(),
				true, null, false );

			void a() => managerTooling.GetRequiredInstance<SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Succeeds()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				false, null, false );

			Assert.NotNull( managerTooling.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>() );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Fails()
		{
			var managerTooling = ManagerTooling.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, false );

			void a() => managerTooling.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void InstantiatingClassIncludedInProcessing_Succeeds()
		{
			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), false, null, false )
				.Register()
				.Configure();

			Assert.NotNull( managerTooling.GetRequiredInstance<SomeInstantiatePerContainerComponent>() );
		}

		[Fact]
		public void InstantiatingClassExcludedFromProcessing_Fails()
		{
			var excludeTypes = new HashSet<Type> { typeof( SomeInstantiatePerContainerComponent ) };

			var managerTooling = new ManagerTooling( Assembly.GetExecutingAssembly(), false, excludeTypes, false )
				.Register()
				.Configure();

			void a() => managerTooling.GetRequiredInstance<SomeInstantiatePerContainerComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		private void VerifyDummies( ManagerTooling managerTooling, bool includeDummyAssembly )
		{
			var scopedProvider = managerTooling.GetOrAddScopedProvider( ScopeNames.Main );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeAttributeWithInstantiatePerScopeAttribute>() );

			Assert.NotNull( managerTooling.GetRequiredInstance<SomeBoundedContext>() );

			Assert.NotNull( managerTooling.GetRequiredInstance<ICommandHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeCommandHandler>() );

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

			Assert.NotNull( managerTooling.GetRequiredInstance<IDomainEventRegistry>() );
			Assert.NotNull( managerTooling.GetRequiredInstance<IDomainEventHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeDomainEventHandler>() );

			Assert.NotNull( managerTooling.GetRequiredInstance<ISomeExternalService>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeInstantiatePerScopeComponent>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeInstantiatePerScopeComponentWithInterface>() );
			Assert.NotNull( managerTooling.GetRequiredInstance<SomeInstantiatePerInjectionComponent>() );
			Assert.NotNull( managerTooling.GetRequiredInstance<ISomeInstantiatePerInjectionComponentWithInterface>() );
			Assert.NotNull( managerTooling.GetRequiredInstance<SomeInstantiatePerContainerComponent>() );
			Assert.NotNull( managerTooling.GetRequiredInstance<ISomeInstantiatePerContainerComponentWithInterface>() );

			Assert.NotNull( managerTooling.GetRequiredInstance<IIntegrationEventHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeIntegrationEventHandler>() );

			var someMessageBusConnection = managerTooling.GetRequiredInstance<ISomeMessageBusConnection>();
			Assert.NotNull( someMessageBusConnection.Connection );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeOutbox>() );
			Assert.NotNull( managerTooling.GetRequiredInstance<IOutboxProcessor>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeProcessableAttributeWithInstantiatePerScopeAttribute>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeTenantComponentLevel1>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeTenantRequestProcessor>() );

			if( includeDummyAssembly )
				VerifyDummiesFromDummyAssembly( managerTooling );
		}

		private void VerifyDummiesFromDummyAssembly( ManagerTooling managerTooling )
		{
			var scopedProvider = managerTooling.GetOrAddScopedProvider( ScopeNames.Main );

			Assert.NotNull( scopedProvider.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>() );
		}
	}
}
