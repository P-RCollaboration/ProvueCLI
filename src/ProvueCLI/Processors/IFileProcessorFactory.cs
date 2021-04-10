namespace ProvueCLI.Processors {

	/// <summary>
	/// Abstract factory for <see cref="IFileProcessor"/>.
	/// </summary>
	public interface IFileProcessorFactory {

		/// <summary>
		/// Create instance <see cref="IFileProcessor"/> based on extension.
		/// </summary>
		/// <param name="extension">Extension.</param>
		IFileProcessor? CreateFileProcessorByExtension ( string extension );

	}

}
