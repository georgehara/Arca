using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Cqrs;
using Microsoft.EntityFrameworkCore;

namespace Automated.Arca.Tests.Dummies
{
	[BoundedContextAttribute( "ConnectionStrings:SomeDatabaseContext", "MigrationsHistory", "MigrationsHistorySchema" )]
	public class SomeBoundedContext : DbContext, IProcessable
	{
		public SomeBoundedContext( DbContextOptions options )
			: base( options )
		{
		}
	}
}
