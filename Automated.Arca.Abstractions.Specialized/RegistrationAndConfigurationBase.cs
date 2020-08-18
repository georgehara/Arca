using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public abstract class RegistrationAndConfigurationBase : DependencyInjection.RegistrationAndConfigurationBase
	{
		protected IInstantiationRegistry SpecializedRegistry( IRegistrationContext context )
		{
			return GetExtensionDependency<IInstantiationRegistry>( context );
		}
	}
}
