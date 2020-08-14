using System;
using System.Collections.Generic;
using System.Reflection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Tests.Dummies;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ManagerTests
	{
		[Fact]
		public void Processing_Succeeds()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, null, false );

			VerifyDummies( applicationPipeline, false );
		}

		[Fact]
		public void ProcessingWithoutDummyAssembly_Fails()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, null, false );

			VerifyDummies( applicationPipeline, false );

			void a() => applicationPipeline.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssembly_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( Assembly.GetExecutingAssembly(), true, null, null, false,
					x => x.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>(),
					x => x.RegisterFirst(),
					x => x.ConfigureFirst() );

			VerifyDummies( applicationPipeline, true );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndConfigure_Fails()
		{
			static void a() => new ApplicationPipeline( Assembly.GetExecutingAssembly(), true, null, null, false,
				x => x.RegisterFirst(),
				x => x.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>(),
				x => x.ConfigureFirst() );

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndRegisterAndConfigure_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( Assembly.GetExecutingAssembly(), true, null, null, false,
				x => { },
				x => x.RegisterFirst()
					.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
					.RegisterSecond(),
				x => x.ConfigureFirst() );

			VerifyDummies( applicationPipeline, true );
		}

		[Fact]
		public void ProcessingWithMultipleCallsToRegisterAndConfigure_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( Assembly.GetExecutingAssembly(), true, null, null, false,
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst()
					.RegisterSecond()
					.ConfigureSecond() );

			VerifyDummies( applicationPipeline, false );
		}

		[Fact]
		public void ProcessingWithMultipleCallsToRegisterAndConfigureAndDummyAssembly_Fails()
		{
			// Microsoft's dependency injection container stops registering components once the service provider is built,
			// without throwing an exception, so trying to register new types after "Configure" was called is pointless.

			var applicationPipeline = new ApplicationPipeline( Assembly.GetExecutingAssembly(), true, null, null, false,
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst()
					.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
					.RegisterSecond()
					.ConfigureSecond() );

			VerifyDummies( applicationPipeline, false );

			void a() => applicationPipeline.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithoutRegister_Fails()
		{
			static void a() => new ApplicationPipeline( Assembly.GetCallingAssembly(), true, null, null, false,
				x => { },
				x => { },
				x => x.ConfigureFirst() );

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithWrongRootAssembly_Fails()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetCallingAssembly(),
				true, null, null, false );

			void a() => applicationPipeline.GetRequiredInstance<SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Succeeds()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				false, null, null, false );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>() );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Fails()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, null, false );

			void a() => applicationPipeline.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void InstantiatingClassIncludedInProcessing_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( Assembly.GetExecutingAssembly(), false, null, null, false,
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst() );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<SomeInstantiatePerContainerComponent>() );
		}

		[Fact]
		public void InstantiatingClassExcludedFromProcessing_Fails()
		{
			var excludeTypes = new HashSet<Type> { typeof( SomeInstantiatePerContainerComponent ) };

			var applicationPipeline = new ApplicationPipeline( Assembly.GetExecutingAssembly(), false, excludeTypes, null, false,
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst() );

			void a() => applicationPipeline.GetRequiredInstance<SomeInstantiatePerContainerComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingPriorityTypes_Succeeds()
		{
			var priorityTypes = new List<Type> { typeof( SomeMiddlewarePerContainer ), typeof( SomeMiddlewarePerScope ),
				typeof( SomeMiddlewarePerInjection ) };

			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( Assembly.GetExecutingAssembly(),
				true, null, priorityTypes, false );

			VerifyDummies( applicationPipeline, false );

			var resultedPriorityTypes = applicationPipeline.GetPriorityTypes();

			Assert.Equal( priorityTypes, resultedPriorityTypes );
		}

		private void VerifyDummies( ApplicationPipeline applicationPipeline, bool includeDummyAssembly )
		{
			var scopedProvider = applicationPipeline.GetOrAddScopedProvider( ScopeNames.Main );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeAttributeWithInstantiatePerScopeAttribute>() );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<SomeBoundedContext>() );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<ICommandHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeCommandHandler>() );

			var someComponentForRegistratorConfigurator
				= scopedProvider.GetRequiredInstance<SomeComponentForRegistratorConfigurator>();
			Assert.True( someComponentForRegistratorConfigurator.Configured );

			// "SomeComponentNotDerivedFromIProcessable" is checked separately.

			var someComponentWithInterfaceSpecifiedInAttribute
				= applicationPipeline.GetRequiredInstance<ISomeComponentWithInterfaceSpecifiedInAttribute>();
			Assert.True( someComponentWithInterfaceSpecifiedInAttribute.Configured );

			var someComponentWithoutInterfaceSpecifiedInAttribute
				= applicationPipeline.GetRequiredInstance<ISomeComponentWithoutInterfaceSpecifiedInAttribute>();
			Assert.True( someComponentWithoutInterfaceSpecifiedInAttribute.Configured );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<IDomainEventRegistry>() );
			Assert.NotNull( applicationPipeline.GetRequiredInstance<IDomainEventHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeDomainEventHandler>() );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<ISomeExternalService>() );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<SomeInstantiatePerContainerComponent>() );
			Assert.NotNull( applicationPipeline.GetRequiredInstance<ISomeInstantiatePerContainerComponentWithInterface>() );
			Assert.NotNull( applicationPipeline.GetRequiredInstance<SomeInstantiatePerInjectionComponent>() );
			Assert.NotNull( applicationPipeline.GetRequiredInstance<ISomeInstantiatePerInjectionComponentWithInterface>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeInstantiatePerScopeComponent>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeInstantiatePerScopeComponentWithInterface>() );

			Assert.NotNull( applicationPipeline.GetRequiredInstance<IIntegrationEventHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeIntegrationEventHandler>() );

			var someMessageBusConnection = applicationPipeline.GetRequiredInstance<ISomeMessageBusConnection>();
			Assert.NotNull( someMessageBusConnection.Connection );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeMiddlewarePerContainer>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeMiddlewarePerInjection>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeMiddlewarePerScope>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeOutbox>() );
			Assert.NotNull( applicationPipeline.GetRequiredInstance<IOutboxProcessor>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeProcessableAttributeWithInstantiatePerScopeAttribute>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeTenantComponentLevel1>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeTenantRequestProcessor>() );

			if( includeDummyAssembly )
				VerifyDummiesFromDummyAssembly( applicationPipeline );
		}

		private void VerifyDummiesFromDummyAssembly( ApplicationPipeline applicationPipeline )
		{
			var scopedProvider = applicationPipeline.GetOrAddScopedProvider( ScopeNames.Main );

			Assert.NotNull( scopedProvider.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>() );
		}
	}
}
