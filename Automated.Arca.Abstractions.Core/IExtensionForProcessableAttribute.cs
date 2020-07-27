using System;

namespace Automated.Arca.Abstractions.Core
{
	public interface IExtensionForProcessableAttribute
	{
		Type AttributeType { get; }

		void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
		void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute );
	}
}
