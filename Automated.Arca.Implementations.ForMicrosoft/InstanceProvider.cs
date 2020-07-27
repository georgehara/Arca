using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public abstract class InstanceProvider : IInstanceProvider
	{
		public InstanceProvider( IServiceProvider dependency )
		{
			Dependency = dependency;
		}

		public IServiceProvider Dependency { get; private set; }

		public object GetInstanceOrNull( Type type )
		{
			return Dependency.GetService( type );
		}

		public T GetInstanceOrNull<T>()
		{
			return Dependency.GetService<T>();
		}

		public object GetRequiredInstance( Type type )
		{
			return Dependency.GetRequiredService( type );
		}

		public T GetRequiredInstance<T>()
		{
			return Dependency.GetRequiredService<T>();
		}
	}
}
