using System;
using System.Reflection;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Libraries;

namespace Automated.Arca.Extensions.Cqrs
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

		public static void SubscribeToMessageBus( IMessageBus messageBus, Type messageType, Type messageListenerType,
			string exchange, string queue )
		{
			messageListenerType.EnsureDerivesFromGenericInterface( typeof( IMessageListener<> ), messageType );

			MethodInfo? method = messageBus.GetType()
				.GetMethod( nameof( IMessageBus.Subscribe ), 2, new Type[] { typeof( string ), typeof( string ) } );

			MethodInfo generic = method!.MakeGenericMethod( messageType, messageListenerType );

			generic!.Invoke( messageBus, new object[] { exchange, queue } );
		}

		public static void SubscribeToMessageBus( IMessageBus messageBus, Type[] messageTypes, Type messageListenerType,
			string exchange, string queue )
		{
			foreach( var messageType in messageTypes )
				SubscribeToMessageBus( messageBus, messageType, messageListenerType, exchange, queue );
		}
	}
}
