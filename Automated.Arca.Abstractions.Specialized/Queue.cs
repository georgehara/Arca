namespace Automated.Arca.Abstractions.Specialized
{
	public static class Queue
	{
		public static string Between( string sourceBoundedContext, string targetBoundedContext ) =>
			$"{sourceBoundedContext}.{targetBoundedContext}";

		public static string For( string boundedContextName ) => boundedContextName;
	}
}
