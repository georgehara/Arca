using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
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

	public class ExtensionForSomeProcessableWithInterfaceAttribute : ExtensionForDependencyInjectionAttribute
	{
		public override Type AttributeType => typeof( SomeProcessableWithInterfaceAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( ISomeConfigurable );

		public ExtensionForSomeProcessableWithInterfaceAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			D.R.InstantiatePerContainer( interfaceType, typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			var instance = (ISomeConfigurable)D.P.GetRequiredInstance( interfaceType );

			instance.Configured = true;
		}
	}
}
