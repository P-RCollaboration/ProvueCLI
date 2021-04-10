using Microsoft.Extensions.DependencyInjection;
using ProvueCLI.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {


	public class FolderProcessor : IFolderProcessor {

		private readonly ILogger m_logger;

		private readonly IServiceProvider m_serviceProvider;

		public FolderProcessor ( ILogger logger , IServiceProvider serviceProvider ) {
			m_logger = logger ?? throw new ArgumentNullException ( nameof ( logger ) );
			m_serviceProvider = serviceProvider ?? throw new ArgumentNullException ( nameof ( serviceProvider ) );
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
			var files = Directory.EnumerateFiles ( fullPath , "*.*" , SearchOption.TopDirectoryOnly )
				.Where (
					a => a.EndsWith ( ".vue" ) ||
					a.EndsWith ( ".html" ) ||
					a.EndsWith ( ".js" ) ||
					a.EndsWith ( ".css" )
				);
			foreach ( var file in files ) {
				var fileName = Path.GetFileName ( file );
				var fileProcessor = FileProcessorFactoryMethod ( fileName );
				if ( fileProcessor == null ) continue;

				m_logger.Log ( $"File is processing {relativeFolder} {fileName}..." );
				await fileProcessor.Process ( Path.Combine ( relativeFolder , fileName ) , sourceFolder , targetFolder );
			}

			return fullPath;
		}

		private IFileProcessor? FileProcessorFactoryMethod ( string fileName ) {
			var extension = Path.GetExtension ( fileName );
			IFileProcessor? processor = null;
			switch ( extension ) {
				case ".vue":
					processor = m_serviceProvider.GetService<IComponentFileProcessor> ();
					break;
				case ".html":
					processor = m_serviceProvider.GetService<IHtmlFileProcessor> ();
					break;
				case ".js":
					processor = m_serviceProvider.GetService<IScriptFileProcessor> ();
					break;
				case ".css":
					processor = m_serviceProvider.GetService<IStyleFileProcessor> ();
					break;
			}

			return processor;
		}

	}

}
