using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ProvueCLI.Commands {
	public class ShowHelp : ICommand {
		public Task Execute(string[] args) {
			Console.WriteLine($"ProvueCLI version {Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? ""}\n");
			Console.WriteLine("Example usage: ProvueCLI <arument1> <arument2> <arument3>\n");
			Console.WriteLine("Options");
			Console.WriteLine("\tsourcefolder:<full or relative path> - specify folder that a contains source code");
			Console.WriteLine("\tbuildfolder:<full or relative path> - specify folder where will be processed source code for development");
			Console.WriteLine("\treleasefolder:<full or relative path> - specify folder where will be processed source code for release");
			Console.WriteLine("\tserverfolder:<full or relative path> - specify folder that will be mapped in web server for static");
			Console.WriteLine("\tport:<full or relative path> - port for web server for static (default 8080)");
			Console.WriteLine("\thost:<full or relative path> - host for web server for static (default localhost)");
			Console.WriteLine("\nCommands");
			Console.WriteLine("\tbuildrelease - indicate that need build release version (you need specify option releasefolder)");
			Console.WriteLine("\trun - indicate that need run web server for static");
			Console.WriteLine("\tcreate - create vuejs single file component");

			return Task.CompletedTask;
		}
	}
}
