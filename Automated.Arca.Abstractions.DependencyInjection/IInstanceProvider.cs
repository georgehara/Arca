using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IInstanceProvider : IExtensionDependency, IDontAutoMock
	{
		object GetInstanceOrNull( Type type );
		T GetInstanceOrNull<T>();
		object GetRequiredInstance( Type type );
		T GetRequiredInstance<T>();
	}
}
