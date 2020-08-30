using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public abstract class MultiInstantiateAttribute : ProcessableWithInterfaceAttribute
	{
		public string ImplementationKey { get; protected set; }

		public MultiInstantiateAttribute( string implementationKey )
		{
			ImplementationKey = implementationKey;
		}

		public MultiInstantiateAttribute( Type interfaceType, string implementationKey )
			: base( interfaceType )
		{
			ImplementationKey = implementationKey;
		}
	}
}
