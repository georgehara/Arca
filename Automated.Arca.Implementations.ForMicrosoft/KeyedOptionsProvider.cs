using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class KeyedOptionsProvider : IKeyedOptionsProvider
	{
		public KeyedOptionsProvider( IConfiguration dependency )
		{
			Dependency = dependency;
		}

		public IConfiguration Dependency { get; private set; }

		public T GetRequiredValue<T>( string keyName )
		{
			if( string.IsNullOrEmpty( keyName ) )
				throw new ArgumentNullException( $"Configuration key is missing." );

			var value = Dependency.GetValue<T>( keyName );

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
