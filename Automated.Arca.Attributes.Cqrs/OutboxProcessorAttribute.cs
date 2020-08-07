﻿using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class OutboxProcessorAttribute : ProcessableWithInterfaceAttribute
	{
		public string ScheduleConfigurationKey { get; protected set; }

		public OutboxProcessorAttribute( string scheduleConfigurationKey )
		{
			ScheduleConfigurationKey = scheduleConfigurationKey;
		}

		public OutboxProcessorAttribute( Type interfaceType, string scheduleConfigurationKey )
			: base( interfaceType )
		{
			ScheduleConfigurationKey = scheduleConfigurationKey;
		}
	}
}