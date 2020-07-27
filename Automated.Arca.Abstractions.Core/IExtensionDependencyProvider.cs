using System;

namespace Automated.Arca.Abstractions.Core
{
	public interface IExtensionDependencyProvider
	{
		object GetExtensionDependency( Type type );
		T GetExtensionDependency<T>();
	}
}
