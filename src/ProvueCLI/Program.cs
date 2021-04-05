using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProvueCLI.Configuration;
using ProvueCLI.PresentationClasses;
using System.Threading.Tasks;

namespace ProvueCLI {

	public class Program {

		private static ApplicationConfiguration m_applicationConfiguration = new ApplicationConfiguration ();

		public static ApplicationConfiguration ApplicationConfiguration => m_applicationConfiguration;

		public static async Task Main ( string[] args ) {
			m_applicationConfiguration = await new ApplicationConfigurationReader ().ReadConfiguration ( args[1..] );

			if ( !m_applicationConfiguration.IsRunDeveloplementServer ) {

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
