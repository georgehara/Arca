using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Automated.Arca.Tests
{
	public class SomeHostedService : IHostedService
	{
		public Task StartAsync( CancellationToken cancellationToken )
		{
			return new Task( () => { } );
		}

		public Task StopAsync( CancellationToken cancellationToken )
		{
			return new Task( () => { } );
		}
	}
}
