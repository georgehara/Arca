using System;
using System.Reflection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Libraries;

namespace Automated.Arca.Extensions.Specialized
{
	public static class GenericsHelper
	{
		public static void RegisterOutboxToOutboxProcessor( IOutboxProcessor outboxProcessor, Type outboxType,
			string boundedContext, OutboxPublicationType publicationType )
		{
			outboxType.EnsureDerivesFromInterface( typeof( IOutbox ) );

			MethodInfo? method = outboxProcessor.GetType()
				.GetMethod( nameof( IOutboxProcessor.Register ), 1,
					new Type[] { typeof( string ), typeof( OutboxPublicationType ) } );

			MethodInfo? generic = method!.MakeGenericMethod( outboxType );

			generic!.Invoke( outboxProcessor, new object[] { boundedContext, publicationType } );
		}

		public static void RegisterMessageToMessageBus( IMessageBus messageBus, Type messageType, Type messageListenerType,
			string exchange, string queue )
		{
			messageType.EnsureDerivesFromInterface( typeof( IMessage ) );
			messageListenerType.EnsureDerivesFromGenericInterfaceNotEqual( typeof( IMessageListener<> ), messageType );

			MethodInfo? method = messageBus.GetType()
				.GetMethod( nameof( IMessageBus.Register ), 2, new Type[] { typeof( string ), typeof( string ) } );

			MethodInfo generic = method!.MakeGenericMethod( messageType, messageListenerType );

			generic!.Invoke( messageBus, new object[] { exchange, queue } );
		}
	}
}
