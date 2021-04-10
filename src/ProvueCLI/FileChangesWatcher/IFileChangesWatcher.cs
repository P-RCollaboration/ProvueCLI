namespace ProvueCLI.FileChangesWatcher {

	/// <summary>
	/// Watcher for changes
	/// </summary>
	public interface IFileChangesWatcher {

		/// <summary>
		/// Watch changes in directory passed via path parameter.
		/// </summary>
		/// <param name="sourceFolder">Source folder.</param>
		/// <param name="targetFolder">Target folder.</param>
		void WatchDirectory ( string sourceFolder , string targetFolder );

	}

}
