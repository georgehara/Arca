using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
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
