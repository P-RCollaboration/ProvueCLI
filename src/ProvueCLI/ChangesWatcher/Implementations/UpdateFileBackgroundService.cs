using ProvueCLI.Loggers;
using ProvueCLI.Processors;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ProvueCLI.ChangesWatcher.Implementations {

	/// <summary>
	/// Background service that take care about updating files.
	/// </summary>
	public class UpdateFileBackgroundService : IUpdateFileBackgroundService {

		private Task? m_currentTask;

		private readonly CancellationTokenSource m_cancellationToken = new ();

		private static readonly ConcurrentQueue<string> m_queue = new ();

		private readonly IFileProcessorFactory m_fileProcessorFactory;

		private readonly ILogger m_logger;

		private string m_sourceFolder = "";

		private string m_targetFolder = "";

		public UpdateFileBackgroundService ( IFileProcessorFactory fileProcessorFactory , ILogger logger ) {
			m_fileProcessorFactory = fileProcessorFactory ?? throw new ArgumentNullException ( nameof ( fileProcessorFactory ) );
			m_logger = logger ?? throw new ArgumentNullException ( nameof ( fileProcessorFactory ) );
		}

		/// <inheritdoc cref="IUpdateFileBackgroundService.SetOptions(string, string)"/>
		public void SetOptions ( string sourceFolder , string targetFolder ) {
			if ( !string.IsNullOrEmpty ( m_sourceFolder ) && !string.IsNullOrEmpty ( m_targetFolder ) ) throw new InvalidOperationException ( "UpdateFileBackgroundService: folders already specified!" );

			m_sourceFolder = sourceFolder;
			m_targetFolder = targetFolder;
		}

		private async Task ExecuteAsync ( CancellationToken stoppingToken ) {
			while ( !stoppingToken.IsCancellationRequested ) {
				var previousPath = "";
				while ( m_queue.TryDequeue ( out var fullPath ) ) {
					if ( previousPath == fullPath ) continue;
					var fileProvider = m_fileProcessorFactory.CreateFileProcessorByExtension ( Path.GetExtension ( fullPath ) );
					if ( fileProvider == null ) return;

					var relativeFilePath = fullPath.Replace ( m_sourceFolder , "" );
					if ( relativeFilePath.StartsWith ( Path.DirectorySeparatorChar ) ) relativeFilePath = relativeFilePath[1..];

					m_logger.Log ( $"File is processing {relativeFilePath}..." );
					try {
						await fileProvider.Process ( relativeFilePath , m_sourceFolder , m_targetFolder );
					} catch ( Exception e ) {
						previousPath = "";
						m_logger.Log ( $"Failed while processing {relativeFilePath} {e.Message}" );
						continue;
					}

					previousPath = fullPath;
				}

				await Task.Delay ( 1000 , stoppingToken );

				previousPath = "";
			}
		}

		public virtual Task StartAsync ( CancellationToken cancellationToken ) {
			m_currentTask = ExecuteAsync ( m_cancellationToken.Token );

			if ( m_currentTask.IsCompleted ) return m_currentTask;

			return Task.CompletedTask;
		}

		public virtual async Task StopAsync ( CancellationToken cancellationToken ) {
			// Stop called without start
			if ( m_currentTask == null ) return;

			try {
				m_cancellationToken.Cancel ();
			} finally {
				// Wait until the task completes or the stop token triggers
				await Task.WhenAny ( m_currentTask , Task.Delay ( Timeout.Infinite , cancellationToken ) );
			}
		}

		public virtual void Dispose () => m_cancellationToken.Cancel ();

		/// <inheritdoc cref="IUpdateFileBackgroundService.Enqueue(string)"/>
		public void Enqueue ( string fullPath ) => m_queue.Enqueue ( fullPath );

	}

}
