using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProvueCLI.FileServices.Implementations {

	public class FileService : IFileService {

		public bool FileExists(string fileName) => File.Exists(fileName);

		public async Task<T> ReadJsonFileAsync<T>(string fileName) {
			using var file = File.OpenRead(fileName);
			var model = await JsonSerializer.DeserializeAsync<T>(
				file ,
				new JsonSerializerOptions {
					PropertyNameCaseInsensitive = true
				}
			);

			if ( model == null ) throw new ArgumentException("File don't meet model structure or contains incorrect content.");

			return model;
		}

	}

}
