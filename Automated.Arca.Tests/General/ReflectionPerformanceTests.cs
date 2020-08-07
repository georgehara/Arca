using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
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
				typeof( SomeExtensionForRegistration ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 70000000;

			for( int i = 0; i < iterations; i++ )
				implementsInterface = typeof( IExtensionForProcessableAttribute ).IsAssignableFrom(
					typeof( SomeExtensionForRegistration ) );

			Trace.WriteLine( $"Speed of '{nameof( TypeDerivesFromInterface )}' = {Speed( sw, iterations )}" );

			Assert.True( implementsInterface );
		}

		[Fact]
		public void TypeDoesntDeriveFromInterface()
		{
			var implementsInterface = typeof( IProcessable ).IsAssignableFrom( typeof( SomeExtensionForRegistration ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 70000000;

			for( int i = 0; i < iterations; i++ )
				implementsInterface = typeof( IProcessable ).IsAssignableFrom( typeof( SomeExtensionForRegistration ) );

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
			var customAttributes = typeof( SomeExtensionForRegistration ).GetCustomAttributes<ProcessableAttribute>();

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 1400000;

			for( int i = 0; i < iterations; i++ )
				customAttributes = typeof( SomeExtensionForRegistration ).GetCustomAttributes<ProcessableAttribute>();

			Trace.WriteLine( $"Speed of '{nameof( GetCustomAttributesWhenDontExist )}' = {Speed( sw, iterations )}" );

			Assert.True( customAttributes.Count() <= 0 );
		}

		[Fact]
		public void ExtractInterfaceMethodFromImplementation()
		{
			var registerMethod = ExtractExtensionForRegistrationMethod( typeof( SomeExtensionForRegistration ) );

			Stopwatch sw = new Stopwatch();
			sw.Start();

			const int iterations = 1000000;

			for( int i = 0; i < iterations; i++ )
				registerMethod = ExtractExtensionForRegistrationMethod( typeof( SomeExtensionForRegistration ) );

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

	public interface IExtensionForRegistration<T> : IExtensionForProcessableAttribute
	{
		void Register( IRegistrationContext context, T attribute, Type typeWithAttribute );
	}

	public class SomeExtensionForRegistration : IExtensionForRegistration<SomeProcessableAttribute>
	{
		public Type AttributeType => typeof( SomeProcessableAttribute );

		public void Register( IRegistrationContext context,
			SomeProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public void Register( IRegistrationContext context,
			ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public void Configure( IConfigurationContext context,
			ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}

	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class SomeProcessableAttribute : ProcessableAttribute
	{
	}
}
