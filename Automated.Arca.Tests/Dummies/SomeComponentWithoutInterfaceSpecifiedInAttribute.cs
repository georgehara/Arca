﻿using System;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeComponentWithoutInterfaceSpecifiedInAttribute : ISomeConfigurable
	{
	}

	[SerializableAttribute] // Used to simulate multiple attributes applied on the class.
	[SomeProcessableAttribute]
	public class SomeComponentWithoutInterfaceSpecifiedInAttribute : ISomeComponentWithoutInterfaceSpecifiedInAttribute
	{
		public bool Configured { get; set; }
	}
}
