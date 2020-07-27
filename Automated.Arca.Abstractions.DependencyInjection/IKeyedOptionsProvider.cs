using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IKeyedOptionsProvider : IExtensionDependency
	{
		T GetRequiredValue<T>( string keyName );
		string GetRequiredString( string keyName );
	}
}
