using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;
using Xunit;

namespace Automated.Arca.Tests
{
	public class MultiImplementationTests
	{
		private const string RightImplementationScopeName = "Tests.RightImplementation";

		[Fact]
		public void MultiImplementationInstance_Succeeds()
		{
			var applicationPipeline = ApplicationPipeline.GetInstanceAndCallRegisterAndConfigure( true, null, null, false,
				null, Assembly.GetExecutingAssembly() );

			var scopedProvider = applicationPipeline.SP( RightImplementationScopeName );

			var leftInstance = applicationPipeline.D.MI<ISomeMultiImplementation>( "Left" );
			var rightInstance = applicationPipeline.D.MI<ISomeMultiImplementation>( scopedProvider, "Right" );

			Assert.Equal( "Left", leftInstance.Name );
			Assert.Equal( "Right", rightInstance.Name );
			Assert.Equal( leftInstance.SomeComponent, rightInstance.SomeComponent );
		}

		[InstantiatePerContainerAttribute]
		public class SomeComponent : IProcessable
		{
		}

		public interface ISomeMultiImplementation : IProcessable
		{
			string Name { get; }
			SomeComponent SomeComponent { get; }
		}

		[MultiInstantiatePerContainerAttribute( typeof( ISomeMultiImplementation ), "Left" )]
		public class SomeLeftImplementation : ISomeMultiImplementation
		{
			public string Name => "Left";
			public SomeComponent SomeComponent { get; }

			public SomeLeftImplementation( SomeComponent someComponent )
			{
				SomeComponent = someComponent;
			}
		}

		[MultiInstantiatePerScopeAttribute( typeof( ISomeMultiImplementation ), "Right" )]
		public class SomeRightImplementation : ISomeMultiImplementation
		{
			public string Name => "Right";
			public SomeComponent SomeComponent { get; }

			public SomeRightImplementation( SomeComponent someComponent )
			{
				SomeComponent = someComponent;
			}
		}
	}
}
