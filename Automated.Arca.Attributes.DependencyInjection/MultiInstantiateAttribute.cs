using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public abstract class MultiInstantiateAttribute : ProcessableAttribute
	{
		public Type InterfaceType { get; protected set; }
		public string ImplementationKey { get; protected set; }

		public MultiInstantiateAttribute( Type interfaceType, string implementationKey )
		{
			InterfaceType = interfaceType;
			ImplementationKey = implementationKey;
		}
	}
}
