using ProvueCLI.Configuration;
using ProvueCLI.PresentationClasses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProvueCLIUnitTest {

	public class ApplicationConfigurationReaderUnitTests {

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_EmptyArguments () {
			// arrange
			var reader = new ApplicationConfigurationReader ();
			var emptyConfiguration = new ApplicationConfiguration ();

			// act
			var configuration = await reader.ReadConfiguration ( Enumerable.Empty<string> () );

			// assert
			Assert.Equal ( emptyConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_Single () {
			// arrange
			var reader = new ApplicationConfigurationReader ();
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\test\source\build", ReleaseFolder = @"c:\test\source\release", SourceFolder = @"c:\test\source" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_BuildFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ();
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\builder" , ReleaseFolder = @"c:\test\source\release" , SourceFolder = @"c:\test\source" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source", @"buildfolder:c:\builder" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_ReleaseFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ();
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\test\source\build" , ReleaseFolder = @"c:\release" , SourceFolder = @"c:\test\source" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source" , @"releasefolder:c:\release" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_BuildFolder_And_ReleaseFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ();
			var testConfiguration = new ApplicationConfiguration { IsRunDeveloplementServer = true };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"run" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

	}

}
