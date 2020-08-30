using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Libraries;
using Automated.Arca.Tests.Dummies;
using Xunit;

namespace Automated.Arca.Tests
{
	public class TypeExtensionsTests
	{
		[Fact]
		public void GetDefaultInterface_SomeClassDerivedFromClass_Succeeds()
		{
			var interfaceType = typeof( SomeClassDerivedFromClass ).GetDefaultInterface();

			Assert.Equal( typeof( ISomeInterface ), interfaceType );
		}

		[Fact]
		public void GetDefaultInterface_SomeClassDerivedFromInterface_Succeeds()
		{
			var interfaceType = typeof( SomeClassDerivedFromInterface ).GetDefaultInterface();

			Assert.Equal( typeof( ISomeInterface ), interfaceType );
		}

		[Fact]
		public void GetDefaultInterface_SomeClassDerivedFromInterfaceWithInterface_Succeeds()
		{
			var interfaceType = typeof( SomeClassDerivedFromInterfaceWithInterface ).GetDefaultInterface();

			Assert.Equal( typeof( ISomeInterfaceDerivedFromInterface ), interfaceType );
		}

		[Fact]
		public void GetDefaultInterface_SomeClassDerivedFromClassWithInterfaceAndInterface_Succeeds()
		{
			var interfaceType = typeof( SomeClassDerivedFromClassWithInterfaceAndInterface ).GetDefaultInterface();

			Assert.Equal( typeof( ISomeInterfaceDerivedFromInterface ), interfaceType );
		}

		[Fact]
		public void GetDefaultInterface_SomeClassDerivedFromClassDerivedFromClassWithInterfaceAndInterface_Succeeds()
		{
			var interfaceType = typeof( SomeClassDerivedFromClassDerivedFromClassWithInterfaceAndInterface ).GetDefaultInterface();

			Assert.Equal( typeof( ISomeInterfaceDerivedFromInterface ), interfaceType );
		}

		[Fact]
		public void GetDefaultInterface_SomeClassDerivedFromInterfaceWithInterfaceAndInterface_Fails()
		{
			static void a() => typeof( SomeClassDerivedFromInterfaceWithInterfaceAndInterface ).GetDefaultInterface();

			Assert.Throws<InvalidOperationException>( a );
		}

		[Fact]
		public void EnsureDerivesFrom_SomeClassDerivedFromClass_Succeeds()
		{
			typeof( SomeClassDerivedFromClass ).EnsureDerivesFromNotEqual( typeof( ISomeInterface ) );
		}

		[Fact]
		public void EnsureDerivesFrom_SomeClassDerivedFromInterface_Succeeds()
		{
			typeof( SomeClassDerivedFromInterface ).EnsureDerivesFromNotEqual( typeof( ISomeInterface ) );
		}

		[Fact]
		public void EnsureDerivesFrom_SomeClassDerivedFromClass_Fails()
		{
			static void a() => typeof( SomeClassDerivedFromClass ).EnsureDerivesFromNotEqual( typeof( ISomeOtherInterface ) );

			Assert.Throws<InvalidCastException>( a );
		}

		[Fact]
		public void EnsureDerivesFromInterface_SomeClassDerivedFromInterface_Succeeds()
		{
			typeof( SomeClassDerivedFromInterface ).EnsureDerivesFromInterface( typeof( ISomeInterface ) );
		}

		[Fact]
		public void EnsureDerivesFromInterface_SomeClassDerivedFromInterfaceWithInterfaceAndInterface_Succeeds()
		{
			typeof( SomeClassDerivedFromInterfaceWithInterfaceAndInterface ).EnsureDerivesFromInterface(
				typeof( ISomeOtherInterface ) );
		}

		[Fact]
		public void EnsureDerivesFromInterface_SomeClassDerivedFromInterface_Fails()
		{
			static void a() => typeof( SomeClassDerivedFromInterface ).EnsureDerivesFromInterface(
				typeof( ISomeOtherInterface ) );

			Assert.Throws<InvalidCastException>( a );
		}

		[Fact]
		public void EnsureDerivesFromGenericInterface_SomeGenericClass_Succeeds()
		{
			typeof( SomeGenericClass ).EnsureDerivesFromGenericInterfaceNotEqual( typeof( ISomeGenericInterface<> )
				, typeof( string ) );
		}

		[Fact]
		public void EnsureDerivesFromGenericInterface_SomeClassDerivedFromClass_Fails()
		{
			static void a() => typeof( SomeClassDerivedFromClass ).EnsureDerivesFromGenericInterfaceNotEqual(
				typeof( ISomeGenericInterface<> ), typeof( string ) );

			Assert.Throws<InvalidCastException>( a );
		}
	}
}
