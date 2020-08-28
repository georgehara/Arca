using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForExternalServiceAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( ExternalServiceAttribute );

		public ExtensionForExternalServiceAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (ExternalServiceAttribute)attribute;

			var interfaceType = attributeTyped.GetInterfaceOrDefault( typeWithAttribute );
			var baseAddressConfigurationKey = attributeTyped.BaseAddressConfigurationKey;
			var baseAddress = D.O.GetRequiredString( baseAddressConfigurationKey );
			var retryCount = attributeTyped.RetryCount;
			var retryDelayMilliseconds = attributeTyped.RetryDelayMilliseconds;
			var circuitBreakerEventCount = attributeTyped.CircuitBreakerEventCount;
			var circuitBreakerDurationSeconds = attributeTyped.CircuitBreakerDurationSeconds;

			S.R.AddExternalService( interfaceType, typeWithAttribute, baseAddress, retryCount, retryDelayMilliseconds,
				circuitBreakerEventCount, circuitBreakerDurationSeconds );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
