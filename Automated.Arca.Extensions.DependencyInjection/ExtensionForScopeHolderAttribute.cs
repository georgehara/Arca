﻿using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeHolderAttribute : ExtensionForInstantiatePerScopeWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeHolderAttribute );
		public override Type? RootInterfaceOfTypeWithAttribute => typeof( IScopeHolder<> );
	}
}
