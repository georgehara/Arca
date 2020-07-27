using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class DomainEventHandlerRegistryAttribute : ProcessableWithInterfaceAttribute
	{
		public DomainEventHandlerRegistryAttribute()
		{
		}

		public DomainEventHandlerRegistryAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
