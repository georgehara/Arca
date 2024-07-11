using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IInstanceProvider : IExtensionDependency
	{
		object GetRequiredInstance( Type type );
		T GetRequiredInstance<T>() where T : notnull;
		object? GetInstanceOrNull( Type type );
		T? GetInstanceOrNull<T>() where T : class;
	}
}
