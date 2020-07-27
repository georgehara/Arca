using System;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeComponentWithInterfaceSpecifiedInAttribute : ISomeConfigurable
	{
	}

	[SomeProcessableAttribute( typeof( ISomeComponentWithInterfaceSpecifiedInAttribute ) )]
	[SerializableAttribute]
	public class SomeComponentWithInterfaceSpecifiedInAttribute : ISomeComponentWithInterfaceSpecifiedInAttribute
	{
		public bool Configured { get; set; }
	}
}
