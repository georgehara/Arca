using System;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeComponentWithoutInterfaceSpecifiedInAttribute : ISomeConfigurable
	{
	}

	[SomeProcessableAttribute]
	[SerializableAttribute]
	public class SomeComponentWithoutInterfaceSpecifiedInAttribute : ISomeComponentWithoutInterfaceSpecifiedInAttribute
	{
		public bool Configured { get; set; }
	}
}
