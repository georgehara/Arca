using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class OutboxProcessorAttribute : ProcessableAttribute
	{
		public string ScheduleConfigurationKey { get; protected set; }

		public OutboxProcessorAttribute( string scheduleConfigurationKey )
		{
			ScheduleConfigurationKey = scheduleConfigurationKey;
		}
	}
}
