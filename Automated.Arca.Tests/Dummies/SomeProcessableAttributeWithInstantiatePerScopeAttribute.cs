using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerScopeAttribute]
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class SomeProcessableAttributeWithInstantiatePerScopeAttribute : ProcessableAttribute, IProcessable
	{
	}
}
