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

		/// <summary>
		/// Write text to file.
		/// </summary>
		/// <param name="filePath">File path.</param>
		/// <param name="text">Text to write.</param>
		/// <returns></returns>
		Task WriteToFile(string filePath, string text);

		/// <summary>
		/// Get current directory path.
		/// </summary>
		/// <returns></returns>
		string GetCurrentDirectory();

		/// <summary>
		/// Get file name from full path.
		/// </summary>
		/// <param name="filePath">File full path.</param>
		/// <returns>File name.</returns>
		string GetFileName(string filePath);

	}

}
