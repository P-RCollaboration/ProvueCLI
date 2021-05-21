using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProvueCLI.Commands;
using ProvueCLI.Configuration;
using ProvueCLI.FileServices.Implementations;
using ProvueCLI.Loggers.Implementations;
using ProvueCLI.PresentationClasses;
using ProvueCLI.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ProvueCLI {

	public class Program {

		private static ApplicationConfiguration m_applicationConfiguration = new ApplicationConfiguration ();

		public static ApplicationConfiguration ApplicationConfiguration => m_applicationConfiguration;

		public static async Task Main ( string[] args ) {
			var logger = new ConsoleLogger ();
			var fileService = new FileService ();
			m_applicationConfiguration = await new ApplicationConfigurationReader ( logger , fileService ).ReadConfiguration ( args );

			var serviceCollection = new ServiceCollection();
			new Startup().ConfigureServices(serviceCollection);
			var serviceProvider = serviceCollection.BuildServiceProvider();

			if (m_applicationConfiguration.IsEmpty()) {
				await new CommandStrategy(logger, new CommandFactory(serviceProvider)).TryExecuteCommand(args);
				return;
			}

			var folderProcessor = serviceProvider.GetService<IFolderProcessor> ();
			if ( folderProcessor != null ) await folderProcessor.ProcessFiles ( m_applicationConfiguration.SourceFolder , m_applicationConfiguration.BuildFolder );

			if ( !Directory.Exists ( m_applicationConfiguration.WebServerFolder ) ) {
				logger.Log ( $"Directory {m_applicationConfiguration.WebServerFolder} is not existed! development server can not be runned!" );
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

		//public async static Task CreateComponent(string fileNameArg) {
		//	var defaultColor = Console.ForegroundColor;

		//	//async component?
		//	var steps = new Dictionary<string , string> {
		//		["styles"] = "Add style section? [Y/n] "
		//	};

		//	var answers = new Dictionary<string , string> {

		//	};

		//	Console.ForegroundColor = ConsoleColor.Green;
		//	Console.WriteLine("Create component file wizard");

		//	try {
		//		var fileNameParts = fileNameArg.Split(Path.DirectorySeparatorChar , StringSplitOptions.RemoveEmptyEntries);
		//		if ( fileNameParts.Length == 0 ) throw new Exception("Incorrect component name");

		//		var componentName = fileNameParts[fileNameParts.Length - 1];
		//		fileNameParts[fileNameParts.Length - 1] += ".vue";

		//		var filePath = Path.Combine(m_applicationConfiguration.SourceFolder);
		//		//filePath = Path.Combine(fileNameParts);
		//		foreach ( var item in fileNameParts ) {
		//			filePath = Path.Combine(filePath, item);
		//		}

		//		//IFileService.FileExists
		//		if ( File.Exists(filePath) ) throw new Exception("Component has already existed");

		//		var templateSection = System.Text.Encoding.ASCII.GetBytes($"<template>\n\t<p>{componentName}</p>\n</template>\n");
		//		var scriptSection = System.Text.Encoding.ASCII.GetBytes($"\n<script>\n" +
		//			"export default async function() {\n" +
		//			"\treturn {\n" +
		//			$"\t\tname: `{componentName}`\n" +
		//			"\t}\n" +
		//			"}\n" +
		//			$"</script>\n");
		//		var stylesSection = System.Text.Encoding.ASCII.GetBytes($"\n<style>\n</style>");

		//		foreach ( var step in steps ) {
		//			Console.Write(step.Value);
		//			answers.Add(step.Key, Console.ReadKey().KeyChar.ToString().ToLower());
		//			Console.WriteLine();
		//		}

		//		using var stream = File.OpenWrite(filePath);

		//		if ( templateSection != null) await stream.WriteAsync(templateSection, 0 , templateSection.Length);
		//		if ( scriptSection != null) await stream.WriteAsync(scriptSection , 0 , scriptSection.Length);
		//		if ( stylesSection != null) await stream.WriteAsync(stylesSection , 0 , stylesSection.Length);


		//		Console.WriteLine($"Component created: {filePath}");

		//	} catch (Exception ex) {
		//		Console.ForegroundColor = ConsoleColor.Red;
		//		Console.WriteLine(ex.Message);
		//	}

		//	Console.ForegroundColor = defaultColor;
		//}

		//private static void ShowHelp() {
  //          Console.WriteLine  ( $"ProvueCLI version {Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? ""}\n" );
  //          Console.WriteLine ( "Example usage: ProvueCLI <arument1> <arument2> <arument3>\n" );
		//	Console.WriteLine ( "Options" );
		//	Console.WriteLine ( "\tsourcefolder:<full or relative path> - specify folder that a contains source code" );
		//	Console.WriteLine ( "\tbuildfolder:<full or relative path> - specify folder where will be processed source code for development" );
		//	Console.WriteLine ( "\treleasefolder:<full or relative path> - specify folder where will be processed source code for release" );
		//	Console.WriteLine ( "\tserverfolder:<full or relative path> - specify folder that will be mapped in web server for static" );
		//	Console.WriteLine ( "\tport:<full or relative path> - port for web server for static (default 8080)" );
		//	Console.WriteLine ( "\thost:<full or relative path> - host for web server for static (default localhost)" );
		//	Console.WriteLine ( "\nCommands" );
		//	Console.WriteLine ( "\tbuildrelease - indicate that need build release version (you need specify option releasefolder)" );
		//	Console.WriteLine ( "\trun - indicate that need run web server for static" );
		//	Console.WriteLine ( "\tcreate - create vuejs single file component" );
		//}

	}
}
