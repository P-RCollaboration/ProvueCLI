namespace ProvueCLI.Processors {

	/// <summary>
	/// Common interface for all file processors.
	/// </summary>
	public interface IFileProcessor {

		/// <summary>
		/// Process file.
		/// </summary>
		/// <param name="fileName">Original file name.</param>
		/// <param name="targetFolder">Root folder where will be store precessed files.</param>
		/// <param name="sourceFolder">The root folder contains original files that used the developer.</param>
		void Process ( string fileName, string sourceFolder, string targetFolder );

	}

}