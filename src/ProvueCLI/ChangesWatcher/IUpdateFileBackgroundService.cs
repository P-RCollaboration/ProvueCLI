using Microsoft.Extensions.Hosting;
using System;

namespace ProvueCLI.ChangesWatcher {

	/// <summary>
	/// Background service that take care about updating files.
	/// </summary>
	public interface IUpdateFileBackgroundService : IHostedService, IDisposable {

		/// <summary>
		/// Set options.
		/// </summary>
		/// <param name="sourceFolder">Source folder.</param>
		/// <param name="targetFolder">Target folder.</param>
		void SetOptions ( string sourceFolder , string targetFolder );

		/// <summary>
		/// Enqueue update for file.
		/// </summary>
		/// <param name="fullPath">Full path.</param>
		void Enqueue ( string fullPath );

	}
}
