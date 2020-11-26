using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Tests.Dummies;
using Xunit;

namespace Automated.Arca.Tests
{
	public class ReflectionPerformanceTests : PerformanceTests
	{
		[Fact]
		public void TypeDerivesFromInterface()
		{
			var implementsInterface = typeof( IExtensionForProcessableAttribute ).IsAssignableFrom(
				typeof( ExtensionForSomeProcessableAttribute ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 70000000;

			for( int i = 0; i < iterations; i++ )
				implementsInterface = typeof( IExtensionForProcessableAttribute ).IsAssignableFrom(
					typeof( ExtensionForSomeProcessableAttribute ) );

			Trace.WriteLine( $"Speed of '{nameof( TypeDerivesFromInterface )}' = {Speed( sw, iterations )}" );

			Assert.True( implementsInterface );
		}

		[Fact]
		public void TypeDoesntDeriveFromInterface()
		{
			var implementsInterface = typeof( IProcessable ).IsAssignableFrom( typeof( ExtensionForSomeProcessableAttribute ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 70000000;

			for( int i = 0; i < iterations; i++ )
				implementsInterface = typeof( IProcessable ).IsAssignableFrom( typeof( ExtensionForSomeProcessableAttribute ) );

			Trace.WriteLine( $"Speed of '{nameof( TypeDoesntDeriveFromInterface )}' = {Speed( sw, iterations )}" );

			Assert.False( implementsInterface );
		}

		[Fact]
		public void GetCustomAttributesWhenExist()
		{
			var customAttributes = typeof( SomeInstantiatePerScopeComponent ).GetCustomAttributes<ProcessableAttribute>();

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 1000000;

			for( int i = 0; i < iterations; i++ )
				customAttributes = typeof( SomeInstantiatePerScopeComponent ).GetCustomAttributes<ProcessableAttribute>();

			Trace.WriteLine( $"Speed of '{nameof( GetCustomAttributesWhenExist )}' = {Speed( sw, iterations )}" );

			Assert.True( customAttributes.Count() > 0 );
		}

		[Fact]
		public void GetCustomAttributesWhenDontExist()
		{
			var customAttributes = typeof( ExtensionForSomeProcessableAttribute ).GetCustomAttributes<ProcessableAttribute>();

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 1400000;

			for( int i = 0; i < iterations; i++ )
				customAttributes = typeof( ExtensionForSomeProcessableAttribute ).GetCustomAttributes<ProcessableAttribute>();

			Trace.WriteLine( $"Speed of '{nameof( GetCustomAttributesWhenDontExist )}' = {Speed( sw, iterations )}" );

			Assert.True( customAttributes.Count() <= 0 );
		}

		[Fact]
		public void ExtractInterfaceMethodFromImplementation()
		{
			var registerMethod = ExtractExtensionForRegistrationMethod( typeof( ExtensionForSomeProcessableAttribute ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 1000000;

			for( int i = 0; i < iterations; i++ )
				registerMethod = ExtractExtensionForRegistrationMethod( typeof( ExtensionForSomeProcessableAttribute ) );

			Trace.WriteLine( $"Speed of '{nameof( ExtractInterfaceMethodFromImplementation )}' = {Speed( sw, iterations )}" );

			Assert.NotNull( registerMethod );
		}

		[Fact]
		public void ExtractInterfaceMethodFromNonImplementation()
		{
			var registerMethod = ExtractExtensionForRegistrationMethod( typeof( SomeInstantiatePerScopeComponent ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 14000000;

			for( int i = 0; i < iterations; i++ )
				registerMethod = ExtractExtensionForRegistrationMethod( typeof( SomeInstantiatePerScopeComponent ) );

			Trace.WriteLine( $"Speed of '{nameof( ExtractInterfaceMethodFromNonImplementation )}' = {Speed( sw, iterations )}" );

			Assert.Null( registerMethod );
		}

		[Fact]
		public void GetDefaultInterfaceFromSomeClassDerivedFromClassDerivedFromClassWithInterfaceAndInterface()
		{
			Type interfaceType;

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 1400000;

			for( int i = 0; i < iterations; i++ )
				interfaceType = typeof( SomeClassDerivedFromClassDerivedFromClassWithInterfaceAndInterface ).GetDefaultInterface();

			Trace.WriteLine( $"Speed of '{nameof( GetDefaultInterfaceFromSomeClassDerivedFromClassDerivedFromClassWithInterfaceAndInterface )}'" +
				$" = {Speed( sw, iterations )}" );
		}

		private MethodInfo? ExtractExtensionForRegistrationMethod( Type implementationType )
		{
			var interfaces = implementationType.GetInterfaces();
			if( interfaces == null || interfaces.Length <= 0 )
				return null;

			var extractedInterface = interfaces.FirstOrDefault( x => x.Name == typeof( IExtensionForRegistration<> ).Name );
			if( extractedInterface == null || extractedInterface.GenericTypeArguments.Length <= 0 )
				return null;

			var genericArgumentType = extractedInterface.GenericTypeArguments[ 0 ];
			if( genericArgumentType == null )
				return null;

			var genericArgumentTypeImplementsProcessableAttribute = typeof( ProcessableAttribute )
				.IsAssignableFrom( genericArgumentType );
			if( !genericArgumentTypeImplementsProcessableAttribute )
				return null;

			var registerMethod = implementationType.GetMethod( nameof( IExtensionForRegistration<int>.Register ),
				new Type[] { typeof( IRegistrationContext ), genericArgumentType, typeof( Type ) } );

			return registerMethod;
		}
	}

	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class SomeProcessableAttribute : ProcessableAttribute
	{
	}

	public interface IExtensionForRegistration<T> : IExtensionForProcessableAttribute
	{
		void Register( IRegistrationContext context, T attribute, Type typeWithAttribute );
	}

	public class ExtensionForSomeProcessableAttribute : ExtensionForDependencyInjectionAttribute,
		IExtensionForRegistration<SomeProcessableAttribute>
	{
		public override Type AttributeType => typeof( SomeProcessableAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => null;

		public ExtensionForSomeProcessableAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public void Register( IRegistrationContext context, SomeProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
