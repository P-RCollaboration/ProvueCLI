using ProvueCLI.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {


	public class FolderProcessor : IFolderProcessor {

		private readonly ILogger m_logger;

		private readonly IFileProcessor m_fileProcessor;

		public FolderProcessor ( ILogger logger , IFileProcessor fileProcessor ) {
			m_logger = logger ?? throw new ArgumentNullException ( nameof ( logger ) );
			m_fileProcessor = fileProcessor ?? throw new ArgumentNullException ( nameof ( fileProcessor ) );
		}

		public async Task ProcessFiles ( string sourceFolder , string targetFolder ) {
			if ( !Directory.Exists ( sourceFolder ) ) {
				m_logger.Log ( "Source or TargetFolder not specified! You need specify it via cli parameters or configuration file!" );
				return;
			}

			var directories = Directory.GetDirectories ( sourceFolder );
			foreach ( var directory in directories ) {
				var directoryName = directory.Split ( Path.DirectorySeparatorChar ).LastOrDefault ();
				if ( directoryName != null ) await ProcessFolder ( sourceFolder , targetFolder , new List<string> { directoryName } );
			}

			await ProcessFilesInFolder ( sourceFolder , targetFolder , new List<string> () );
		}

		private async Task ProcessFolder ( string sourceFolder , string targetFolder , IEnumerable<string> pathSegments ) {
			string fullPath = await ProcessFilesInFolder ( sourceFolder , targetFolder , pathSegments );

			var directories = Directory.GetDirectories ( fullPath );
			foreach ( var directory in directories ) {
				var directoryName = directory.Split ( Path.DirectorySeparatorChar ).LastOrDefault ();
				if ( directoryName != null ) {
					var segments = pathSegments.ToList ();
					segments.Add ( directoryName );
					await ProcessFolder ( sourceFolder , targetFolder , segments );
				}
			}
		}

		private async Task<string> ProcessFilesInFolder ( string sourceFolder , string targetFolder , IEnumerable<string> pathSegments ) {
			var relativeFolder = string.Join ( '/' , pathSegments );
			var fullPath = Path.Combine ( sourceFolder , relativeFolder );
			var files = Directory.GetFiles ( fullPath , "*.vue" );
			foreach ( var file in files ) {
				var fileName = Path.GetFileName ( file );
				m_logger.Log ( $"File is processing {relativeFolder} {fileName}..." );
				await m_fileProcessor.Process ( Path.Combine ( relativeFolder , fileName ) , sourceFolder , targetFolder );
			}

			return fullPath;
		}
	}

}
