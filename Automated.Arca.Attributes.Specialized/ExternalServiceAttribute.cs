using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class ExternalServiceAttribute : ProcessableWithInterfaceAttribute
	{
		public string BaseAddressConfigurationKey { get; protected set; }
		public int RetryCount { get; protected set; }
		public int RetryDelayMilliseconds { get; protected set; }
		public int CircuitBreakerEventCount { get; protected set; }
		public int CircuitBreakerDurationSeconds { get; protected set; }

		public ExternalServiceAttribute( string baseAddressConfigurationKey, int retryCount = 3,
			int retryDelayMilliseconds = 500, int circuitBreakerEventCount = 5, int circuitBreakerDurationSeconds = 30 )
		{
			BaseAddressConfigurationKey = baseAddressConfigurationKey;
			RetryCount = retryCount;
			RetryDelayMilliseconds = retryDelayMilliseconds;
			CircuitBreakerEventCount = circuitBreakerEventCount;
			CircuitBreakerDurationSeconds = circuitBreakerDurationSeconds;
		}

		public ExternalServiceAttribute( Type interfaceType, string baseAddressConfigurationKey, int retryCount = 3,
				int retryDelayMilliseconds = 500, int circuitBreakerEventCount = 5, int circuitBreakerDurationSeconds = 30 )
			: base( interfaceType )
		{
			BaseAddressConfigurationKey = baseAddressConfigurationKey;
			RetryCount = retryCount;
			RetryDelayMilliseconds = retryDelayMilliseconds;
			CircuitBreakerEventCount = circuitBreakerEventCount;
			CircuitBreakerDurationSeconds = circuitBreakerDurationSeconds;
		}
	}
}
