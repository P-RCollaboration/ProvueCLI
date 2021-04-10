using ProvueCLI.Processors;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProvueCLI.FileChangesWatcher.Implementations {

	/// <inheritdoc cref="IFileChangesWatcher" />
	public class FileChangesWatcher : IFileChangesWatcher {

		private static FileSystemWatcher? m_fileSystemWatcher;

		private readonly IFileProcessorFactory m_fileProcessorFactory;

		private string m_sourceFolder = "";

		private string m_targetFolder = "";

		public FileChangesWatcher ( IFileProcessorFactory fileProcessorFactory ) {
			m_fileProcessorFactory = fileProcessorFactory ?? throw new ArgumentNullException ( nameof ( fileProcessorFactory ) );
		}

		/// <inheritdoc cref="IFileChangesWatcher.WatchDirectory(string, string)" />
		public void WatchDirectory ( string sourceFolder , string targetFolder ) {
			if ( m_fileSystemWatcher != null ) throw new Exception ( "Watcher already started!" );

			m_fileSystemWatcher = new FileSystemWatcher ( @"C:\path\to\folder" );

			m_sourceFolder = sourceFolder;
			m_targetFolder = targetFolder;

			m_fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
				| NotifyFilters.CreationTime
				| NotifyFilters.DirectoryName
				| NotifyFilters.FileName
				| NotifyFilters.LastWrite
				| NotifyFilters.Size;

			m_fileSystemWatcher.Changed += OnChanged;
			m_fileSystemWatcher.Created += OnCreated;
			m_fileSystemWatcher.Deleted += OnDeleted;
			m_fileSystemWatcher.Renamed += OnRenamed;

			m_fileSystemWatcher.Filter = "*.*";
			m_fileSystemWatcher.IncludeSubdirectories = true;
			m_fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void OnRenamed ( object sender , RenamedEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;


		}

		private void OnDeleted ( object sender , FileSystemEventArgs e ) {
			//TODO: delete file in build folder
		}

		private async void OnCreated ( object sender , FileSystemEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;

			await UpdateFile ( e.FullPath );
		}

		private async void OnChanged ( object sender , FileSystemEventArgs e ) {
			if ( !Path.HasExtension ( e.FullPath ) ) return;

			await UpdateFile ( e.FullPath );
		}

		private async Task UpdateFile ( string fullPath ) {
			var fileProvider = m_fileProcessorFactory.CreateFileProcessorByExtension ( Path.GetExtension ( fullPath ) );
			if ( fileProvider == null ) return;

			await fileProvider.Process ( fullPath.Replace ( m_sourceFolder , "" ) , m_sourceFolder , m_targetFolder );
		}

	}

}
