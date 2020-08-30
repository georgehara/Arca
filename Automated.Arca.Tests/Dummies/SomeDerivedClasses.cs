namespace Automated.Arca.Tests.Dummies
{
	public class SomeClassDerivedFromClass : SomeClassDerivedFromInterface
	{
	}

	public class SomeClassDerivedFromInterfaceWithInterface : ISomeInterfaceDerivedFromInterface
	{
	}

	public class SomeClassDerivedFromClassWithInterfaceAndInterface : SomeClassDerivedFromInterface,
		ISomeInterfaceDerivedFromInterface
	{
	}

	public class SomeClassDerivedFromClassDerivedFromClassWithInterfaceAndInterface :
		SomeClassDerivedFromClassWithInterfaceAndInterface
	{
	}

	public class SomeClassDerivedFromInterface : ISomeInterface
	{
	}

	public interface ISomeInterface
	{
	}

	public interface ISomeInterfaceDerivedFromInterface : ISomeOtherInterface
	{
	}

	public interface ISomeOtherInterface
	{
	}

	public class SomeClassDerivedFromInterfaceWithInterfaceAndInterface : ISomeInterfaceDerivedFromInterface,
		ISomeInterfaceToFail
	{
	}

	public interface ISomeInterfaceToFail
	{
	}

	public class SomeGenericClass : ISomeGenericInterface<string>
	{
	}

	public interface ISomeGenericInterface<T>
	{
	}
}
