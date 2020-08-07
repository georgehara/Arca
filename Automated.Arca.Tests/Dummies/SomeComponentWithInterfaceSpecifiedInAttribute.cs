using System;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeComponentWithInterfaceSpecifiedInAttribute : ISomeConfigurable
	{
	}

	[SerializableAttribute] // Used to simulate multiple attributes applied on the class.
	[SomeProcessableAttribute( typeof( ISomeComponentWithInterfaceSpecifiedInAttribute ) )]
	public class SomeComponentWithInterfaceSpecifiedInAttribute : ISomeComponentWithInterfaceSpecifiedInAttribute
	{
		public bool Configured { get; set; }
	}
}
