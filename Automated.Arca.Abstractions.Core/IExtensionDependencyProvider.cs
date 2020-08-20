using System;

namespace Automated.Arca.Abstractions.Core
{
	public interface IExtensionDependencyProvider
	{
		bool ContainsExtensionDependency( Type type );
		object GetExtensionDependency( Type type );
		T GetExtensionDependency<T>();
	}
}
