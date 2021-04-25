using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ProvueCLI.ChangesWatcher;
using ProvueCLI.ChangesWatcher.Implementations;
using ProvueCLI.Configuration;
using ProvueCLI.FileServices;
using ProvueCLI.Loggers;
using ProvueCLI.Loggers.Implementations;
using ProvueCLI.Processors;
using ProvueCLI.Processors.Implementations;
using System.IO;
using ProvueCLI.FileServices.Implementations;
using System;
using Microsoft.Net.Http.Headers;

namespace ProvueCLI {

	public class Startup {

		public void ConfigureServices ( IServiceCollection services ) {
			services.AddTransient<ApplicationConfigurationReader> ();
			services.AddTransient<ITemplateProcessor , TemplateProcessor> ();
			services.AddTransient<IScriptProcessor , ScriptProcessor> ();
			services.AddTransient<IStyleProcessor , StyleProcessor> ();
			services.AddTransient<ILogger , ConsoleLogger> ();
			services.AddTransient<IFolderProcessor , FolderProcessor> ();
			services.AddTransient<IComponentFileProcessor , ComponentFileProcessor> ();
			services.AddTransient<IHtmlFileProcessor , HtmlFileProcessor> ();
			services.AddTransient<IStyleFileProcessor , StyleFileProcessor> ();
			services.AddTransient<IScriptFileProcessor , ScriptFileProcessor> ();
			services.AddTransient<IImageFileProcessor , ImageFileProcessor> ();
			services.AddSingleton<IFileChangesWatcher , FileChangesWatcher> ();
			services.AddTransient<IFileProcessorFactory , FileProcessorFactory> ();
			services.AddTransient<IFileService , FileService> ();
			services.AddSingleton<IUpdateFileBackgroundService , UpdateFileBackgroundService> ();
			services.AddHostedService (
				( serviceProvider ) => {
					var service = serviceProvider.GetService<IUpdateFileBackgroundService> ();
					if ( service == null ) throw new InvalidProgramException ( "UpdateFileBackgroundService don't resolved!" );

					return (UpdateFileBackgroundService) service;
				}
			);
		}

		public void Configure ( IApplicationBuilder app , IWebHostEnvironment env , IFileChangesWatcher fileChangesWatcher , IUpdateFileBackgroundService updateFileBackgroundService ) {
			if ( env.IsDevelopment () ) app.UseDeveloperExceptionPage ();

			DefaultFilesOptions options = new DefaultFilesOptions ();
			options.DefaultFileNames.Clear ();
			app.UseDefaultFiles ( options );

			app.UseStaticFiles (
				new StaticFileOptions () {
					FileProvider = new PhysicalFileProvider (
						Path.GetFullPath ( Program.ApplicationConfiguration.WebServerFolder )
					) ,
					RequestPath = new PathString ( "/dir" ) ,
					ServeUnknownFileTypes = true
				}
			);

			app.UseRouting ();

			app.UseEndpoints (
				endpoints => {
					endpoints.MapGet (
						"/" ,
						async context => {
							await context.Response.WriteAsync ( "Provue development server" );
						}
					);
				}
			);

			fileChangesWatcher.WatchDirectory ( Program.ApplicationConfiguration.SourceFolder , Program.ApplicationConfiguration.BuildFolder );
			updateFileBackgroundService.SetOptions ( Program.ApplicationConfiguration.SourceFolder , Program.ApplicationConfiguration.BuildFolder );
		}
	}
}
