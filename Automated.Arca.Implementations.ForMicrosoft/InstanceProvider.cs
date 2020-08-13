using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public abstract class InstanceProvider : IInstanceProvider
	{
		protected IServiceProvider ServiceProvider { get; private set; }

		public InstanceProvider( IServiceProvider serviceProvider )
		{
			ServiceProvider = serviceProvider;
		}

		public object GetInstanceOrNull( Type type )
		{
			return ServiceProvider.GetService( type );
		}

		public T GetInstanceOrNull<T>()
		{
			return ServiceProvider.GetService<T>();
		}

		public object GetRequiredInstance( Type type )
		{
			return ServiceProvider.GetRequiredService( type );
		}

		public T GetRequiredInstance<T>()
		{
			return ServiceProvider.GetRequiredService<T>();
		}
	}
}
