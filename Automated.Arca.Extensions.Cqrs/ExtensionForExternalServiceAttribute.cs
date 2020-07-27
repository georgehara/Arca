using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Extensions.Cqrs
{
	public class ExtensionForExternalServiceAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( ExternalServiceAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (ExternalServiceAttribute)attribute;

			var interfaceType = attributeTyped.GetDefaultInterface( typeWithAttribute );
			var baseAddressConfigurationKey = attributeTyped.BaseAddressConfigurationKey;
			var baseAddress = Options( context ).GetRequiredString( baseAddressConfigurationKey );
			var retryCount = attributeTyped.RetryCount;
			var retryDelayMilliseconds = attributeTyped.RetryDelayMilliseconds;
			var circuitBreakerEventCount = attributeTyped.CircuitBreakerEventCount;
			var circuitBreakerDurationSeconds = attributeTyped.CircuitBreakerDurationSeconds;

			Registry( context ).AddExternalService( interfaceType, typeWithAttribute,
				baseAddress, retryCount, retryDelayMilliseconds, circuitBreakerEventCount, circuitBreakerDurationSeconds );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
