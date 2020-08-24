using System;
using Automated.Arca.Libraries;

namespace Automated.Arca.Abstractions.Core
{
	public static class AttributeExtensions
	{
		public static Type GetInterfaceOrDefault( this ProcessableWithInterfaceAttribute attribute, Type typeWithAttribute )
		{
			if( attribute.InterfaceType != null )
			{
				typeWithAttribute.EnsureDerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( attribute.InterfaceType );

				return attribute.InterfaceType;
			}

			return typeWithAttribute.GetDefaultInterface();
		}
	}
}
