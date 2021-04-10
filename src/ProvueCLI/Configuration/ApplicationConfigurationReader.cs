﻿using ProvueCLI.Loggers;
using ProvueCLI.PresentationClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProvueCLI.Configuration {

	/// <summary>
	/// Class can read configuration from provue configuration files.
	/// </summary>
	public class ApplicationConfigurationReader {

		private string m_mainDirectoryCommand = Directory.GetCurrentDirectory ();

		private const string SourceFolderArgument = "sourcefolder";

		private const string BuildFolderArgument = "buildfolder";

		private const string WebServerFolderArgument = "serverfolder";

		private const string RunDeveloplementServer = "run";

		private const string WebServerPort = "port";

		private const string WebServerHost = "host";

		private const string ReleaseFolder = "releasefolder";

		private const string BuildRelease = "buildrelease";

		private readonly ILogger m_logger;

		public ApplicationConfigurationReader ( ILogger logger ) {
			m_logger = logger;
		}

		public async Task<ApplicationConfiguration> ReadConfiguration ( IEnumerable<string> arguments ) {
			ApplicationConfiguration configuration = new ();
			var directoryConfigurationFile = Path.Combine ( m_mainDirectoryCommand , "provue" );

			if ( File.Exists ( directoryConfigurationFile ) ) {
				try {
					configuration = await ReadConfigurationFile ( directoryConfigurationFile );
				} catch {
					m_logger.Log ( $"Can't read configuration file on path {directoryConfigurationFile}! Check that file is correct." );
				}
			}

			foreach ( var argument in arguments ) {
				configuration = ParseArgument ( argument , configuration );
			}

			return configuration;
		}

		private ApplicationConfiguration ParseArgument ( string argument , ApplicationConfiguration applicationConfiguration ) {
			var argumentSpan = argument.AsSpan ();
			var separator = argumentSpan.IndexOf ( ":" , StringComparison.InvariantCulture );
			ReadOnlySpan<char> argumentType;
			ReadOnlySpan<char> argumentValue;
			if ( separator > -1 ) {
				argumentType = argumentSpan.Slice ( 0 , separator );
				argumentValue = argumentSpan[( separator + 1 )..];
			} else {
				argumentType = argumentSpan;
				argumentValue = "";
			}

			switch ( argumentType ) {
				case var sourceFolder when MemoryExtensions.Equals ( sourceFolder , SourceFolderArgument , StringComparison.Ordinal ):
					return ParseSourceFolder ( argumentValue , applicationConfiguration );
				case var buildFolder when MemoryExtensions.Equals ( buildFolder , BuildFolderArgument , StringComparison.Ordinal ):
					return ParseBuildFolder ( argumentValue , applicationConfiguration );
				case var runDevelopmentServer when MemoryExtensions.Equals ( runDevelopmentServer , RunDeveloplementServer , StringComparison.Ordinal ):
					return ParseRunDevelopmentServer ( argumentValue , applicationConfiguration );
				case var webServerHost when MemoryExtensions.Equals ( webServerHost , WebServerHost , StringComparison.Ordinal ):
					return ParseWebServerHost ( argumentValue , applicationConfiguration );
				case var webServerPort when MemoryExtensions.Equals ( webServerPort , WebServerPort , StringComparison.Ordinal ):
					return ParseWebServerPort ( argumentValue , applicationConfiguration );
				case var releaseFolder when MemoryExtensions.Equals ( releaseFolder , ReleaseFolder , StringComparison.Ordinal ):
					return ParseReleaseFolder ( argumentValue , applicationConfiguration );
				case var buildRelease when MemoryExtensions.Equals ( buildRelease , BuildRelease , StringComparison.Ordinal ):
					return ParseBuildRelease ( argumentValue , applicationConfiguration );
				case var webServerFolderArgument when MemoryExtensions.Equals ( webServerFolderArgument , WebServerFolderArgument , StringComparison.Ordinal ):
					return ParseWebServerFolder ( argumentValue , applicationConfiguration );
			}

			return applicationConfiguration;
		}

		private ApplicationConfiguration ParseWebServerFolder ( ReadOnlySpan<char> argumentValue , ApplicationConfiguration applicationConfiguration ) {
			return applicationConfiguration with { WebServerFolder = argumentValue.ToString () };
		}

		private ApplicationConfiguration ParseBuildRelease ( ReadOnlySpan<char> argumentValue , ApplicationConfiguration applicationConfiguration ) {
			return applicationConfiguration with { BuildForRelease = true };
		}

		private ApplicationConfiguration ParseReleaseFolder ( ReadOnlySpan<char> argumentValue , ApplicationConfiguration applicationConfiguration ) {
			return applicationConfiguration with { ReleaseFolder = argumentValue.ToString () };
		}

		private ApplicationConfiguration ParseWebServerPort ( ReadOnlySpan<char> value , ApplicationConfiguration applicationConfiguration ) {
			if ( value.Length == 0 ) {
				Console.WriteLine ( $"Port must be specified! Format port:<number>. Example port:8080." );
				throw new ArgumentException ( nameof ( value ) );
			}

			var port = Convert.ToInt32 ( value.ToString () );

			if ( port < 1000 ) {
				Console.WriteLine ( $"Port specified via argument incorrect {value.ToString ()}! Port can be integer value greater then 1000." );
				throw new ArgumentException ( nameof ( value ) );
			}
			if ( port > 65535 ) {
				Console.WriteLine ( $"Port specified via argument incorrect {value.ToString ()}! Port can be integer value less then 65536." );
				throw new ArgumentException ( nameof ( value ) );
			}

			return applicationConfiguration with { WebServerPort = port };
		}

		private ApplicationConfiguration ParseWebServerHost ( ReadOnlySpan<char> value , ApplicationConfiguration applicationConfiguration ) {
			return applicationConfiguration with { WebServerHost = value.ToString () };
		}

		private ApplicationConfiguration ParseRunDevelopmentServer ( ReadOnlySpan<char> value , ApplicationConfiguration applicationConfiguration ) {
			return applicationConfiguration with { IsRunDeveloplementServer = true };
		}

		private ApplicationConfiguration ParseSourceFolder ( ReadOnlySpan<char> value , ApplicationConfiguration applicationConfiguration ) {
			if ( string.IsNullOrEmpty ( applicationConfiguration.BuildFolder ) && string.IsNullOrEmpty ( applicationConfiguration.ReleaseFolder ) && string.IsNullOrEmpty ( applicationConfiguration.WebServerFolder ) ) {
				return applicationConfiguration with
				{
					BuildFolder = Path.Combine ( value.ToString () , "build" ) ,
					ReleaseFolder = Path.Combine ( value.ToString () , "release" ) ,
					SourceFolder = value.ToString () ,
					WebServerFolder = value.ToString ()
				};
			}

			if ( string.IsNullOrEmpty ( applicationConfiguration.BuildFolder ) ) {

				return applicationConfiguration with
				{
					BuildFolder = Path.Combine ( value.ToString () , "build" ) ,
					SourceFolder = value.ToString ()
				};
			}

			if ( string.IsNullOrEmpty ( applicationConfiguration.ReleaseFolder ) ) {

				return applicationConfiguration with
				{
					BuildFolder = Path.Combine ( value.ToString () , "release" ) ,
					SourceFolder = value.ToString ()
				};
			}

			if ( string.IsNullOrEmpty ( applicationConfiguration.WebServerFolder ) ) {

				return applicationConfiguration with
				{
					WebServerFolder = value.ToString () ,
					SourceFolder = value.ToString ()
				};
			}

			return applicationConfiguration with { SourceFolder = value.ToString () };
		}

		private ApplicationConfiguration ParseBuildFolder ( ReadOnlySpan<char> value , ApplicationConfiguration applicationConfiguration ) {
			return applicationConfiguration with { BuildFolder = value.ToString () };
		}

		private async Task<ApplicationConfiguration> ReadConfigurationFile ( string configurationFile ) {
			using ( var file = File.OpenRead ( configurationFile ) ) {
				var configuration = await JsonSerializer.DeserializeAsync<ApplicationConfiguration> ( file );
				return configuration ?? new ApplicationConfiguration ();
			}
		}

	}

}
