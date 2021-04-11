using ProvueCLI.Loggers;
using ProvueCLI.Processors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProvueCLI.ChangesWatcher.Implementations {

	/// <inheritdoc cref="IFileChangesWatcher" />
	public class FileChangesWatcher : IFileChangesWatcher {

		private FileSystemWatcher? m_fileSystemWatcher;

		private readonly IFileProcessorFactory m_fileProcessorFactory;

		private readonly ILogger m_logger;

		private string m_sourceFolder = "";

		private string m_targetFolder = "";

		private ConcurrentDictionary<string , bool> m_filesInProcess = new ConcurrentDictionary<string , bool> ();

		public FileChangesWatcher ( IFileProcessorFactory fileProcessorFactory , ILogger logger ) {
			m_fileProcessorFactory = fileProcessorFactory ?? throw new ArgumentNullException ( nameof ( fileProcessorFactory ) );
			m_logger = logger ?? throw new ArgumentNullException ( nameof ( logger ) );
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

			m_sourceFolder = sourceFolder;
			m_targetFolder = targetFolder;

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

		private async void OnRenamed ( object sender , RenamedEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;

			await UpdateFile ( e.FullPath );
		}

		private async void OnChanged ( object sender , FileSystemEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;

			await UpdateFile ( e.FullPath );
		}

		private async Task UpdateFile ( string fullPath ) {
			if ( m_filesInProcess.ContainsKey ( fullPath ) ) return;

			if ( !m_filesInProcess.TryAdd ( fullPath , true ) ) return;

			try {
				var fileProvider = m_fileProcessorFactory.CreateFileProcessorByExtension ( Path.GetExtension ( fullPath ) );
				if ( fileProvider == null ) return;

				var relativeFilePath = fullPath.Replace ( m_sourceFolder , "" );
				if ( relativeFilePath.StartsWith ( Path.DirectorySeparatorChar ) ) relativeFilePath = relativeFilePath.Substring ( 1 );

				m_logger.Log ( $"File is processing {relativeFilePath}..." );

				await fileProvider.Process ( relativeFilePath , m_sourceFolder , m_targetFolder );
			} catch {
				m_logger.Log ( $"File is processed with errors!" );
			} finally {
				m_filesInProcess.TryRemove ( new KeyValuePair<string , bool> ( fullPath , true ) );
			}
		}

	}

}
