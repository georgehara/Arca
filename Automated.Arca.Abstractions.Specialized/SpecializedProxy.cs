using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public class SpecializedProxy : ISpecializedProxy
	{
		public IExtensionDependencyProvider ExtensionDependencyProvider { get; }

		public object X( Type type ) => ExtensionDependencyProvider.GetExtensionDependency( type );
		public T X<T>() => ExtensionDependencyProvider.GetExtensionDependency<T>();

		public ISpecializedRegistry R => X<ISpecializedRegistry>();
		public IMiddlewareRegistry M => X<IMiddlewareRegistry>();

		public SpecializedProxy( IExtensionDependencyProvider extensionDependencyProvider )
		{
			ExtensionDependencyProvider = extensionDependencyProvider;
		}
	}
}
