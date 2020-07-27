namespace Automated.Arca.Abstractions.Core
{
	public interface IProcessingContext : IExtensionDependencyProvider
	{
		IExtensionDependencyProvider ExtensionDependencyProvider { get; }
	}
}
