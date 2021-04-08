using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ProvueCLI.Configuration;
using ProvueCLI.Loggers;
using ProvueCLI.Loggers.Implementations;
using ProvueCLI.Processors;
using ProvueCLI.Processors.Implementations;

namespace ProvueCLI {

	public class Startup {

		public void ConfigureServices ( IServiceCollection services ) {
			services.AddTransient<ApplicationConfigurationReader> ();
			services.AddTransient<ITemplateProcessor , TemplateProcessor> ();
			services.AddTransient<IScriptProcessor , ScriptProcessor> ();
			services.AddTransient<IStyleProcessor , StyleProcessor> ();
			services.AddTransient<ILogger , ConsoleLogger> ();
			services.AddTransient<IFolderProcessor , FolderProcessor> ();
			services.AddTransient<IFileProcessor , ComponentProcessor> ();
		}

		public void Configure ( IApplicationBuilder app , IWebHostEnvironment env ) {
			if ( env.IsDevelopment () ) app.UseDeveloperExceptionPage ();

			DefaultFilesOptions options = new DefaultFilesOptions ();
			options.DefaultFileNames.Clear ();
			app.UseDefaultFiles ( options );

			app.UseStaticFiles (
				new StaticFileOptions () {
					FileProvider = new PhysicalFileProvider (
						Program.ApplicationConfiguration.BuildFolder
					) ,
					RequestPath = new PathString ( "/" ) ,
					ServeUnknownFileTypes = true
				}
			);
			app.UseRouting ();

			/*app.UseEndpoints ( endpoints => {
				endpoints.MapGet ( "/" , async context => {
					await context.Response.WriteAsync ( "Provue development server" );
				} );
			} );*/
		}
	}
}
