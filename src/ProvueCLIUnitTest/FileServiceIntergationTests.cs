using ProvueCLI.FileServices.Implementations;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ProvueCLITests {
	public class FileServiceIntergationTests {

		[Fact]
		[Trait("Category", "Integration")]
		public void FileExists_Positive() {
			// arrange
			var fileService = new FileService ();
			var tempFileName = Path.GetTempFileName () + Guid.NewGuid ();
			File.WriteAllText ( tempFileName, "" );

			// act
			var result = fileService.FileExists ( tempFileName );

			//assert
			File.Delete ( tempFileName );
			Assert.True ( result );
		}

		[Fact]
		[Trait ( "Category" , "Integration" )]
		public void FileExists_Negative () {
			// arrange
			var fileService = new FileService ();
			var tempFileName = Path.GetTempFileName () + Guid.NewGuid();

			// act
			var result = fileService.FileExists ( tempFileName );

			//assert
			Assert.False ( result );
		}

		private record TestModel {
			public string Name { get; set; }
		}

		[Fact]
		[Trait ( "Category" , "Integration" )]
		public async Task ReadJsonFileAsync_Throw_FileNotFound () {
			// arrange
			var fileService = new FileService ();
			var tempFileName = Path.GetTempFileName () + Guid.NewGuid ();

			// assert
			await Assert.ThrowsAsync<FileNotFoundException> (
				async () => {
					// act
					await fileService.ReadJsonFileAsync<TestModel> ( tempFileName );
				}
			);
		}

		[Fact]
		[Trait ( "Category" , "Integration" )]
		public async Task ReadJsonFileAsync_ReadModel () {
			// arrange
			var fileService = new FileService ();
			var tempFileName = Path.GetTempFileName () + Guid.NewGuid ();
			await File.WriteAllTextAsync ( tempFileName , "{\"name\": \"Testerka\"}" );
			var testModel = new TestModel { Name = "Testerka" };

			// act
			var model = await fileService.ReadJsonFileAsync<TestModel> ( tempFileName );
			
			// assert
			File.Delete ( tempFileName );
			Assert.Equal ( testModel , model );
		}

		[Fact]
		[Trait ( "Category" , "Integration" )]
		public async Task ReadJsonFileAsync_ReadModel_Throw_InnorrectModel () {
			// arrange
			var fileService = new FileService ();
			var tempFileName = Path.GetTempFileName () + Guid.NewGuid ();
			await File.WriteAllTextAsync ( tempFileName , "[]" );

			// assert
			await Assert.ThrowsAsync<JsonException> (
				async () => {
					// act
					await fileService.ReadJsonFileAsync<TestModel> ( tempFileName );
				}
			);

			File.Delete ( tempFileName );
		}

		[Fact]
		[Trait("Category", "Integration")]
		public void GetFileNameFromPathWithOutExtension_Positive()
		{
			// arrange
			var fileService = new FileService();
			var tempFileName = Guid.NewGuid().ToString();
			var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);
			File.Create(tempFilePath).Close();

			// assert
			Assert.Equal(fileService.GetFileName(tempFilePath), tempFileName);

			File.Delete(tempFilePath);
		}

		[Fact]
		[Trait("Category", "Integration")]
		public void GetFileNameFromPathWithExtension_Positive()
		{
			// arrange
			var fileService = new FileService();
			var tempFileName = Guid.NewGuid().ToString();
			var tempFileNameWithExtension = tempFileName + ".vue";
			var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileNameWithExtension);
			File.Create(tempFilePath).Close();

			// assert
			Assert.Equal(fileService.GetFileName(tempFilePath), tempFileName);

			File.Delete(tempFilePath);
		}

		[Fact]
		[Trait("Category", "Integration")]
		public async Task WriteToFile_Positive()
		{
			// arrange
			var fileService = new FileService();
			var tempFileName = Guid.NewGuid().ToString();
			var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);
			var tempText = "test some file writes";
			await fileService.WriteToFile(tempFilePath, tempText);

			var result = await File.ReadAllTextAsync(tempFilePath);
			// assert
			Assert.Equal(result, tempText);

			File.Delete(tempFilePath);
		}

	}

}
