namespace Automated.Arca.Abstractions.Cqrs
{
	public static class Exchange
	{
		public static string PublicationsFor( string boundedContextName ) => $"{boundedContextName}PublicationExchange";
		public static string CommandsFor( string boundedContextName ) => $"{boundedContextName}CommandExchange";
	}
}
