using System.Net.Http;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeExternalService : IProcessable
	{
	}

	[ExternalServiceAttribute( typeof( ISomeExternalService ), "ExternalServices:SomeExternalService" )]
	public class SomeExternalService : ISomeExternalService
	{
		public HttpClient Client;

		// The parameter "HttpClient" is required by dependency injection; other dependency injection parameters may follow.
		public SomeExternalService( HttpClient client )
		{
			Client = client;
		}
	}
}
