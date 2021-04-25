using ProvueCLI.Loggers;
using System;
using System.IO;

namespace ProvueCLI.ChangesWatcher.Implementations {

	/// <inheritdoc cref="IFileChangesWatcher" />
	public class FileChangesWatcher : IFileChangesWatcher {

		private FileSystemWatcher? m_fileSystemWatcher;

		private readonly ILogger m_logger;

		private readonly IUpdateFileBackgroundService m_updateFileBackgroundService;

		public FileChangesWatcher ( ILogger logger, IUpdateFileBackgroundService updateFileBackgroundService ) {
			m_logger = logger ?? throw new ArgumentNullException ( nameof ( logger ) );
			m_updateFileBackgroundService = updateFileBackgroundService ?? throw new ArgumentNullException ( nameof ( updateFileBackgroundService ) );
		}

		/// <inheritdoc cref="IFileChangesWatcher.WatchDirectory(string, string)" />
		public void WatchDirectory ( string sourceFolder , string targetFolder ) {
			if ( m_fileSystemWatcher != null ) {
				var message = "Watcher already started!";
				m_logger.Log ( message );
				throw new Exception ( message );
			}

			m_logger.Log ( $"Filesystem watcher started in folder: {sourceFolder}" );

			m_fileSystemWatcher = new FileSystemWatcher ( sourceFolder );

			m_fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime
				| NotifyFilters.DirectoryName
				| NotifyFilters.FileName
				| NotifyFilters.LastWrite;

			m_fileSystemWatcher.Changed += OnChanged;
			m_fileSystemWatcher.Renamed += OnRenamed;

			m_fileSystemWatcher.Filter = "*.*";
			m_fileSystemWatcher.IncludeSubdirectories = true;
			m_fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void OnRenamed ( object sender , RenamedEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;

			UpdateFile ( e.FullPath );
		}

		private void OnChanged ( object sender , FileSystemEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;

			UpdateFile ( e.FullPath );
		}

		private void UpdateFile ( string fullPath ) => m_updateFileBackgroundService.Enqueue ( fullPath );

	}

}
