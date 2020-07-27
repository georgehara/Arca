using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class IntegrationEventHandlerRegistryAttribute : ProcessableWithInterfaceAttribute
	{
		public IntegrationEventHandlerRegistryAttribute()
		{
		}

		public IntegrationEventHandlerRegistryAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
