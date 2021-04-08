using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProvueCLI.Configuration;
using ProvueCLI.Loggers.Implementations;
using ProvueCLI.PresentationClasses;
using ProvueCLI.Processors;
using System.Threading.Tasks;

namespace ProvueCLI {

	public class Program {

		private static ApplicationConfiguration m_applicationConfiguration = new ApplicationConfiguration ();

		public static ApplicationConfiguration ApplicationConfiguration => m_applicationConfiguration;

		public static async Task Main ( string[] args ) {
			m_applicationConfiguration = await new ApplicationConfigurationReader ( new ConsoleLogger () ).ReadConfiguration ( args );

			if ( !m_applicationConfiguration.IsRunDeveloplementServer ) {
				var serviceCollection = new ServiceCollection ();
				new Startup ().ConfigureServices ( serviceCollection );
				var serviceProvider = serviceCollection.BuildServiceProvider ();

				var folderProcessor = serviceProvider.GetService<IFolderProcessor> ();
				if ( folderProcessor != null ) await folderProcessor.ProcessFiles ( m_applicationConfiguration.SourceFolder , m_applicationConfiguration.BuildFolder );

				return;
			}

			CreateHostBuilder ( args ).Build ().Run ();
		}

		public static IHostBuilder CreateHostBuilder ( string[] args ) =>
			Host.CreateDefaultBuilder ( args )
				.ConfigureWebHostDefaults (
					webBuilder => {
						webBuilder.UseUrls ( $"http://{m_applicationConfiguration.WebServerHost}:{m_applicationConfiguration.WebServerPort}" );
						webBuilder.UseStartup<Startup> ();
					}
				);
	}
}
