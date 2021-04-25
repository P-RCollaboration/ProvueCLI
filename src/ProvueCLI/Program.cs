using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProvueCLI.Configuration;
using ProvueCLI.FileServices.Implementations;
using ProvueCLI.Loggers.Implementations;
using ProvueCLI.PresentationClasses;
using ProvueCLI.Processors;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProvueCLI {

	public class Program {

		private static ApplicationConfiguration m_applicationConfiguration = new ApplicationConfiguration ();

		public static ApplicationConfiguration ApplicationConfiguration => m_applicationConfiguration;

		public static async Task Main ( string[] args ) {
			var logger = new ConsoleLogger ();
			var fileService = new FileService ();
			m_applicationConfiguration = await new ApplicationConfigurationReader ( logger , fileService ).ReadConfiguration ( args );

			if (m_applicationConfiguration.IsEmpty()) {
				ShowHelp ();
				return;
			}

			var serviceCollection = new ServiceCollection ();
			new Startup ().ConfigureServices ( serviceCollection );
			var serviceProvider = serviceCollection.BuildServiceProvider ();

			var folderProcessor = serviceProvider.GetService<IFolderProcessor> ();
			if ( folderProcessor != null ) await folderProcessor.ProcessFiles ( m_applicationConfiguration.SourceFolder , m_applicationConfiguration.BuildFolder );

			if ( !Directory.Exists ( m_applicationConfiguration.WebServerFolder ) ) {
				logger.Log ( $"Directory {m_applicationConfiguration.WebServerFolder} not exists! development server can runned!" );
				return;
			}
			if ( m_applicationConfiguration.IsRunDeveloplementServer ) CreateHostBuilder ( args ).Build ().Run ();
		}

		public static IHostBuilder CreateHostBuilder ( string[] args ) =>
			Host.CreateDefaultBuilder ( args )
				.ConfigureWebHostDefaults (
					webBuilder => {
						webBuilder.UseUrls ( $"http://{m_applicationConfiguration.WebServerHost}:{m_applicationConfiguration.WebServerPort}" );
						webBuilder.UseStartup<Startup> ();
					}
				);

		private static void ShowHelp() {
			Console.WriteLine ( "ProvueCLI version 0.0.0\n" );
			Console.WriteLine ( "Example usage: ProvueCLI <arument1> <arument2> <arument3>\n" );
			Console.WriteLine ( "Options" );
			Console.WriteLine ( "\tsourcefolder:<full or relative path> - specify folder that a contains source code" );
			Console.WriteLine ( "\tbuildfolder:<full or relative path> - specify folder where will be processed source code for development" );
			Console.WriteLine ( "\treleasefolder:<full or relative path> - specify folder where will be processed source code for release" );
			Console.WriteLine ( "\tserverfolder:<full or relative path> - specify folder that will be mapped in web server for static" );
			Console.WriteLine ( "\tport:<full or relative path> - port for web server for static (default 8080)" );
			Console.WriteLine ( "\thost:<full or relative path> - host for web server for static (default localhost)" );
			Console.WriteLine ( "\nCommands" );
			Console.WriteLine ( "\tbuildrelease - indicate that need build release version (you need specify option releasefolder)" );
			Console.WriteLine ( "\trun - indicate that need run web server for static" );
		}

	}
}
