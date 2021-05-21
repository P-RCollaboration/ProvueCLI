using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProvueCLI.FileServices.Implementations {

	/// <inheritdoc cref="IFileService"/>
	public class FileService : IFileService {

		/// <inheritdoc cref="IFileService.FileExists(string)"/>
		public bool FileExists ( string fileName ) => File.Exists ( fileName );

		/// <inheritdoc cref="IFileService.ReadJsonFileAsync{T}(string)"/>
		public async Task<T> ReadJsonFileAsync<T> ( string fileName ) {
			using var file = File.OpenRead ( fileName );
			var model = await JsonSerializer.DeserializeAsync<T> (
				file ,
				new JsonSerializerOptions {
					PropertyNameCaseInsensitive = true
				}
			);

			if ( model == null ) throw new ArgumentException ( "File don't meet model structure or contains incorrect content." );

			return model;
		}

		public async Task WriteToFile(string filePath, string text) {
			var buffer = Encoding.UTF8.GetBytes ( text );
			using var stream = File.OpenWrite (filePath);
			await stream.WriteAsync (buffer , 0 , buffer.Length );
		}

		public string GetCurrentDirectory() => Directory.GetCurrentDirectory();

		public string GetFileName(string filePath) {
			var fileInfo = new FileInfo(filePath);
			var indexOfExtension = fileInfo.Name.LastIndexOf(fileInfo.Extension);

			return indexOfExtension == -1 ? fileInfo.Name : fileInfo.Name.Substring(0, indexOfExtension);
		}

	}

}
