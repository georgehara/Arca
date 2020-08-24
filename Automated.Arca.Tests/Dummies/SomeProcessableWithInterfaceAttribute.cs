using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Libraries;

namespace Automated.Arca.Tests.Dummies
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class SomeProcessableWithInterfaceAttribute : ProcessableWithInterfaceAttribute
	{
		public SomeProcessableWithInterfaceAttribute()
		{
		}

		public SomeProcessableWithInterfaceAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}

	public class ExtensionForSomeProcessableWithInterfaceAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( SomeProcessableWithInterfaceAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			typeWithAttribute.EnsureDerivesFromInterface( interfaceType );

			ToInstantiatePerContainer( context, interfaceType, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			typeWithAttribute.EnsureDerivesFromInterface( interfaceType );
			interfaceType.EnsureDerivesFromInterface( typeof( ISomeConfigurable ) );

			var instance = (ISomeConfigurable)GetRequiredInstance( context, interfaceType );

			instance.Configured = true;
		}
	}
}
