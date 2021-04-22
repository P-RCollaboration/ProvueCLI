using FakeItEasy;
using ProvueCLI.Configuration;
using ProvueCLI.FileServices;
using ProvueCLI.Loggers;
using ProvueCLI.PresentationClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProvueCLIUnitTest {

	public class ApplicationConfigurationReaderUnitTests {

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_EmptyArguments () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var emptyConfiguration = new ApplicationConfiguration ();

			// act
			var configuration = await reader.ReadConfiguration ( Enumerable.Empty<string> () );

			// assert
			Assert.Equal ( emptyConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void Constructor_Throw_Logger_Null () {
			// assert
			Assert.Throws<ArgumentNullException> (
				() => {
					// arrange and act
					var reader = new ApplicationConfigurationReader ( null , A.Fake<IFileService> () );
				}
			);
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void Constructor_Throw_FileService_Null () {
			// assert
			Assert.Throws<ArgumentNullException> (
				() => {
					// arrange and act
					var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , null );
				}
			);
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_Single () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\test\source\build" , ReleaseFolder = @"c:\test\source\release" , SourceFolder = @"c:\test\source" , WebServerFolder = @"c:\test\source" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_BuildFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\builder" , ReleaseFolder = @"c:\test\source\release" , SourceFolder = @"c:\test\source" , WebServerFolder = @"c:\test\source" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source" , @"buildfolder:c:\builder" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_ReleaseFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\test\source\build" , ReleaseFolder = @"c:\release" , SourceFolder = @"c:\test\source" , WebServerFolder = @"c:\test\source" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source" , @"releasefolder:c:\release" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_WebServerFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { BuildFolder = @"c:\test\source\build" , ReleaseFolder = @"c:\test\source\release" , SourceFolder = @"c:\test\source" , WebServerFolder = @"c:\test\server" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"sourcefolder:c:\test\source" , @"serverfolder:c:\test\server" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_SourceFolderArgument_And_BuildFolder_And_ReleaseFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { IsRunDeveloplementServer = true };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"run" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_WebServerHost () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { WebServerHost = "127.0.0.1" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"host:127.0.0.1" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_WebServerPort () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { WebServerPort = 8080 };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"port:8080" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Theory]
		[InlineData ( "" )]
		[InlineData ( "0" )]
		[InlineData ( "80" )]
		[InlineData ( "200" )]
		[InlineData ( "-1" )]
		[InlineData ( "65545" )]
		[InlineData ( "74500" )]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_WebServerPort_Throw_OutOfRange ( string port ) {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );

			// assert
			await Assert.ThrowsAsync<ArgumentException> (
				async () => {
					// act
					var configuration = await reader.ReadConfiguration ( new List<string> { @"port:" + port } );
				}
			);
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_WebServerFolder () {
			// arrange
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , A.Fake<IFileService> () );
			var testConfiguration = new ApplicationConfiguration { WebServerFolder = @"C:\nega\nebulus" };

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"serverfolder:C:\nega\nebulus" } );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_ReadFromFile () {
			// arrange
			var fileService = A.Fake<IFileService> ();
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , fileService );
			var fileName = Path.Combine ( Directory.GetCurrentDirectory () , "provue" );
			var testConfiguration = new ApplicationConfiguration { WebServerFolder = @"C:\nega\nebulus" };
			A.CallTo ( () => fileService.ReadJsonFileAsync<ApplicationConfiguration> ( fileName ) ).Returns ( Task.FromResult ( testConfiguration ) );
			A.CallTo ( () => fileService.FileExists ( fileName ) ).Returns ( true );

			// act
			var configuration = await reader.ReadConfiguration ( Enumerable.Empty<string> () );

			// assert
			Assert.Equal ( testConfiguration , configuration );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public async Task ReadConfiguration_ReadFromFile_Overlap_CliArguments () {
			// arrange
			var fileService = A.Fake<IFileService> ();
			var reader = new ApplicationConfigurationReader ( A.Fake<ILogger> () , fileService );
			var fileName = Path.Combine ( Directory.GetCurrentDirectory () , "provue" );
			var testConfiguration = new ApplicationConfiguration { WebServerFolder = @"C:\nega\nebulus" };
			var overlapedConfiguration = new ApplicationConfiguration { WebServerFolder = @"C:\great\wall" };
			A.CallTo ( () => fileService.ReadJsonFileAsync<ApplicationConfiguration> ( fileName ) ).Returns ( Task.FromResult ( testConfiguration ) );
			A.CallTo ( () => fileService.FileExists ( fileName ) ).Returns ( true );

			// act
			var configuration = await reader.ReadConfiguration ( new List<string> { @"serverfolder:C:\great\wall" } );

			// assert
			Assert.Equal ( overlapedConfiguration , configuration );
		}

	}

}
