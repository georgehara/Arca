using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class KeyedOptionsProvider : IKeyedOptionsProvider
	{
		protected IConfiguration ApplicationOptionsProvider { get; private set; }

		public KeyedOptionsProvider( IConfiguration applicationOptionsProvider )
		{
			ApplicationOptionsProvider = applicationOptionsProvider;
		}

		public T GetRequiredValue<T>( string keyName )
		{
			if( string.IsNullOrEmpty( keyName ) )
				throw new ArgumentNullException( $"Configuration key is missing." );

			var value = ApplicationOptionsProvider.GetValue<T>( keyName );

			if( value == null )
				throw new InvalidOperationException( $"Configuration value for key '{keyName}' is missing, but is required." );

			return value;
		}

		public string GetRequiredString( string keyName )
		{
			return GetRequiredValue<string>( keyName );
		}
	}
}
