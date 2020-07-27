using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class DomainEventRegistryAttribute : ProcessableWithInterfaceAttribute
	{
		public DomainEventRegistryAttribute()
		{
		}

		public DomainEventRegistryAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
