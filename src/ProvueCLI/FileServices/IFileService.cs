using System.Threading.Tasks;

namespace ProvueCLI.FileServices {

	/// <summary>
	/// Service contains functionality for work with file system.
	/// </summary>
	public interface IFileService {

		/// <summary>
		/// Read file and deserialize content to json.
		/// </summary>
		/// <typeparam name="T">Model for deserialization.</typeparam>
		/// <param name="fileName">File name.</param>
		/// <returns></returns>
		Task<T> ReadJsonFileAsync<T> ( string fileName );

		/// <summary>
		/// Make sure the file exists.
		/// </summary>
		/// <param name="fileName">File name.</param>
		bool FileExists ( string fileName );

	}

}
