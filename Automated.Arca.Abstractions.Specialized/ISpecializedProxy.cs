using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface ISpecializedProxy : IExtensionDependency
	{
		ISpecializedRegistry R { get; }
		IMiddlewareRegistry M { get; }
	}
}
