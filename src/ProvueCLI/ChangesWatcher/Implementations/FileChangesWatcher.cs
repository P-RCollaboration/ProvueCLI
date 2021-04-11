using ProvueCLI.Loggers;
using ProvueCLI.Processors;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ProvueCLI.ChangesWatcher.Implementations {

	/// <inheritdoc cref="IFileChangesWatcher" />
	public class FileChangesWatcher : IFileChangesWatcher {

		private FileSystemWatcher? m_fileSystemWatcher;

		private readonly IFileProcessorFactory m_fileProcessorFactory;

		private readonly ILogger m_logger;

		private string m_sourceFolder = "";

		private string m_targetFolder = "";

		private SemaphoreSlim m_semaphore = new SemaphoreSlim ( 1 , 1 );

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
			await m_semaphore.WaitAsync ();

			try {
				await Task.Delay ( 100 ); // I added this because events fires twice on changes and I don't understand why it is happened
				var fileProvider = m_fileProcessorFactory.CreateFileProcessorByExtension ( Path.GetExtension ( fullPath ) );
				if ( fileProvider == null ) return;

				var relativeFilePath = fullPath.Replace ( m_sourceFolder , "" );
				if ( relativeFilePath.StartsWith ( Path.DirectorySeparatorChar ) ) relativeFilePath = relativeFilePath.Substring ( 1 );

				m_logger.Log ( $"File is processing {relativeFilePath}..." );

				await fileProvider.Process ( relativeFilePath , m_sourceFolder , m_targetFolder );
			} catch {
				m_logger.Log ( $"File is processed with errors!" );
			} finally {
				m_semaphore.Release ();
			}
		}

	}

}
