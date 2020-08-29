using System;
using System.Collections.Generic;
using System.Reflection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Tests.Dummies;
using NSubstitute;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ManagerTests
	{
		[Fact]
		public void Processing_Succeeds()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, null, false,
				null, Assembly.GetExecutingAssembly() );

			VerifyDummies( applicationPipeline, false );
		}

		[Fact]
		public void ProcessingWithoutDummyAssembly_Fails()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, null, false,
				null, Assembly.GetExecutingAssembly() );

			VerifyDummies( applicationPipeline, false );

			void a() => applicationPipeline.D.P.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssembly_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( x => { }, true, true, null, null, false, false,
				null, Assembly.GetExecutingAssembly(),
				x => x.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>(),
				x => x.RegisterFirst(),
				x => x.ConfigureFirst() );

			VerifyDummies( applicationPipeline, true );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndConfigure_Fails()
		{
			static void a() => new ApplicationPipeline( x => { }, true, true, null, null, false, false,
				null, Assembly.GetExecutingAssembly(),
				x => { },
				x => x.RegisterFirst()
					.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>(),
				x => x.ConfigureFirst() );

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithDummyAssemblyBetweenRegisterAndRegisterAndConfigure_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( x => { }, true, true, null, null, false, false,
				null, Assembly.GetExecutingAssembly(),
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
			var applicationPipeline = new ApplicationPipeline( x => { }, true, true, null, null, false, false,
				null, Assembly.GetExecutingAssembly(),
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
			// without throwing an exception, so it's pointless to register new types after "Configure" is called.

			var applicationPipeline = new ApplicationPipeline( x => { }, true, true, null, null, false, false,
				null, Assembly.GetExecutingAssembly(),
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst()
					.AddAssemblyContainingType<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>()
					.RegisterSecond()
					.ConfigureSecond() );

			VerifyDummies( applicationPipeline, false );

			void a() => applicationPipeline.D.P.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithoutRegister_Fails()
		{
			static void a() => new ApplicationPipeline( x => { }, true, true, null, null, false, false,
				null, Assembly.GetCallingAssembly(),
				x => { },
				x => { },
				x => x.ConfigureFirst() );

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingWithWrongRootAssembly_Fails()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, null, false,
				null, Assembly.GetCallingAssembly() );

			void a() => applicationPipeline.D.P.GetRequiredInstance<SomeInstantiatePerScopeComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Succeeds()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( false, null, null, false,
				null, Assembly.GetExecutingAssembly() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>() );
		}

		[Fact]
		public void ProcessingNotDerivedFromIProcessable_Fails()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, null, false,
				null, Assembly.GetExecutingAssembly() );

			void a() => applicationPipeline.D.P.GetRequiredInstance<SomeComponentNotDerivedFromIProcessable>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void InstantiatingClassIncludedInProcessing_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( x => { }, true, false, null, null, false, false,
				null, Assembly.GetExecutingAssembly(),
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeInstantiatePerContainerComponent>() );
		}

		[Fact]
		public void InstantiatingClassExcludedFromProcessing_Fails()
		{
			var excludeTypes = new HashSet<Type> { typeof( SomeInstantiatePerContainerComponent ) };

			var applicationPipeline = new ApplicationPipeline( x => { }, true, false, excludeTypes, null, false, false,
				null, Assembly.GetExecutingAssembly(),
				x => { },
				x => x.RegisterFirst(),
				x => x.ConfigureFirst() );

			void a() => applicationPipeline.D.P.GetRequiredInstance<SomeInstantiatePerContainerComponent>();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void ProcessingPriorityTypes_Succeeds()
		{
			var priorityTypes = new List<Type> { typeof( SomeMiddlewarePerContainer ), typeof( SomeMiddlewarePerScope ),
				typeof( SomeMiddlewarePerInjection ) };

			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, priorityTypes, false,
				null, Assembly.GetExecutingAssembly() );

			VerifyDummies( applicationPipeline, false );

			var resultedPriorityTypes = applicationPipeline.GetPriorityTypes();

			Assert.Equal( priorityTypes, resultedPriorityTypes );
		}

		[Fact]
		public void AutomatedMocking_Succeeds()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, null, false,
				new AutomatedMocker(), Assembly.GetExecutingAssembly() );

			VerifyDummies( applicationPipeline, false );
		}

		[Fact]
		public void AutomatedAndManualMocking_Succeeds()
		{
			var applicationPipeline = new ApplicationPipeline( x => { }, true, false, null, null, false, false,
				new AutomatedMocker(), Assembly.GetExecutingAssembly(),
				x => { },
				x => x
					.RegisterFirst()
					.WithManualMocking( ir =>
					{
						var mock = Substitute.For<ISomeComponentToSubstitute>();
						mock.Get( Arg.Any<string>() ).ReturnsForAnyArgs( "Substituted value" );
						ir.InstantiatePerScope( typeof( ISomeComponentToSubstitute ), sp => mock, true );
					} ),
				x => x.ConfigureFirst() );

			VerifyDummies( applicationPipeline, false );

			var scopedProvider = applicationPipeline.SP( ScopeNames.ManualMocking );
			var instance = scopedProvider.GetRequiredInstance<ISomeComponentToSubstitute>();

			var value = instance.Get( "Some key" );

			Assert.Equal( "Substituted value", value );
		}

		[Fact]
		public void SampleForAutomatedAndManualMocking()
		{
			AutomatedAndManualMocking_Succeeds();
		}

		private void VerifyDummies( ApplicationPipeline applicationPipeline, bool includeDummyAssembly )
		{
			var scopedProvider = applicationPipeline.SP( ScopeNames.Main );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeAttributeWithInstantiatePerScopeAttribute>() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeBoundedContext>() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<ICommandHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeCommandHandler>() );

			var someComponentForRegistratorConfigurator
				= applicationPipeline.D.P.GetRequiredInstance<SomeComponentForRegistratorConfigurator>();
			Assert.True( someComponentForRegistratorConfigurator.Configured );

			// "SomeComponentNotDerivedFromIProcessable" is checked separately.

			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeComponentToSubstitute>() );

			var someComponentWithInterfaceSpecifiedInAttribute
				= applicationPipeline.D.P.GetRequiredInstance<ISomeComponentWithInterfaceSpecifiedInAttribute>();
			Assert.True( someComponentWithInterfaceSpecifiedInAttribute.Configured );

			var someComponentWithoutInterfaceSpecifiedInAttribute
				= applicationPipeline.D.P.GetRequiredInstance<ISomeComponentWithoutInterfaceSpecifiedInAttribute>();
			Assert.True( someComponentWithoutInterfaceSpecifiedInAttribute.Configured );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<IDomainEventRegistry>() );
			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<IDomainEventHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeDomainEventHandler>() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<ISomeExternalService>() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeInstantiatePerContainerComponent>() );
			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<ISomeInstantiatePerContainerComponentWithInterface>() );
			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeInstantiatePerInjectionComponent>() );
			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<ISomeInstantiatePerInjectionComponentWithInterface>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeInstantiatePerScopeComponent>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeInstantiatePerScopeComponentWithInterface>() );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<IIntegrationEventHandlerRegistry>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeIntegrationEventHandler>() );

			var someMessageBusConnection = applicationPipeline.D.P.GetRequiredInstance<ISomeMessageBusConnection>();
			Assert.NotNull( someMessageBusConnection.Connection );

			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeMiddlewarePerContainer>() );
			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<SomeMiddlewarePerInjection>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeMiddlewarePerScope>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeOutbox>() );
			Assert.NotNull( applicationPipeline.D.P.GetRequiredInstance<IOutboxProcessor>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeProcessableAttributeWithInstantiatePerScopeAttribute>() );

			Assert.NotNull( scopedProvider.GetRequiredInstance<SomeTenantComponentLevel1>() );
			Assert.NotNull( scopedProvider.GetRequiredInstance<ISomeTenantRequestProcessor>() );

			if( includeDummyAssembly )
				VerifyDummiesFromDummyAssembly( applicationPipeline );
		}

		private void VerifyDummiesFromDummyAssembly( ApplicationPipeline applicationPipeline )
		{
			var scopedProvider = applicationPipeline.SP( ScopeNames.Main );

			Assert.NotNull( scopedProvider.GetRequiredInstance<global::Tests.DummyAssembly.SomeInstantiatePerScopeComponent>() );
		}
	}
}
