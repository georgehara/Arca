using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IMultiImplementationRegistry : IExtensionDependency
	{
		void Add( Type interfaceType, string implementationKey, Type implementationType );
		Type GetRequiredImplementationType( Type interfaceType, string implementationKey );
	}
}
