using ProvueCLI.FileServices;
using ProvueCLI.PresentationClasses.Commands;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProvueCLI.Commands {
	public class CreateComponent : ICommand {

		private readonly IFileService m_fileService;
		private const string m_templateSection = "<template {nameSpace}>\n\t<p>{componentName}</p>\n</template>\n";
		private const string m_scriptSection = "\n<script>\nexport default {async}function() {\n\treturn {\n\t\tname: `{componentName}`\n\t}\n}\n</script>\n";
		private const string m_stylesSection = "\n<style>\n</style>";

		public CreateComponent(IFileService fileService) {
			m_fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
		}

		public string CombineFilePath(string fileName) {
			var fileNameParts = fileName.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
			if ( fileNameParts.Length == 0 ) throw new Exception("Incorrect component name");

			fileNameParts[fileNameParts.Length - 1] += ".vue";

			var filePath = Path.Combine(m_fileService.GetCurrentDirectory(), Path.Combine(fileNameParts));

			if ( m_fileService.FileExists(filePath) ) throw new Exception("Component has already existed");

			return filePath;
		}

		public CreateCommandConfiguration GetAnswersFromConsole() {
			var model = new CreateCommandConfiguration();
			foreach ( var step in model.m_steps ) {
				Console.Write(step.Key);
				if ( !step.Value.Invoke(Console.ReadLine() ?? "") ) throw new Exception($"Step: {step.Key} failed.");
			}

			return model;
		}

		public string GenerateTemplate(CreateCommandConfiguration config, string componentName) {
			var templateBuilder = new StringBuilder(m_templateSection);

			templateBuilder.Replace("{nameSpace}", string.IsNullOrEmpty(config.ComponentNamespace) ? "" : $"@=\"{config.ComponentNamespace}\"");
			templateBuilder.Append(m_scriptSection.Replace("{async}", config.IsComponentAsync ? "async " : ""));
			if ( config.HasStyleSection ) templateBuilder.Append(m_stylesSection);

			templateBuilder.Replace("{componentName}", componentName);

			return templateBuilder.ToString();
		}

		public async Task Execute(string[] args) {
			var defaultColor = Console.ForegroundColor;

			try {
				var filePath = CombineFilePath(args[1]);
				var componentName = m_fileService.GetFileName(filePath);
				var config = GetAnswersFromConsole();

				var template = GenerateTemplate(config, componentName);

				await m_fileService.WriteToFile(filePath, template);

				Console.WriteLine($"Component created: {filePath}");

			} catch ( Exception ex ) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
			}

			Console.ForegroundColor = defaultColor;
		}

	}
}
