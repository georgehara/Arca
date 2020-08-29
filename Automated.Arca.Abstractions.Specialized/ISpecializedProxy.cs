using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface ISpecializedProxy : IExtensionDependency, IExtensionDependencyProviderContainer
	{
		ISpecializedRegistry R { get; }
		IMiddlewareRegistry M { get; }
	}
}
