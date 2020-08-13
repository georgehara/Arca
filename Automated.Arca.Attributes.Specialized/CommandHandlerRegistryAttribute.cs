using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class CommandHandlerRegistryAttribute : ProcessableWithInterfaceAttribute
	{
		public CommandHandlerRegistryAttribute()
		{
		}

		public CommandHandlerRegistryAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
