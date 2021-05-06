using ProvueCLI.Loggers;
using ProvueCLI.Processors.PresentationClasses;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <summary>
	/// Process .vue files.
	/// </summary>
	public class ComponentFileProcessor : IComponentFileProcessor {

		private readonly IScriptProcessor m_scriptProcessor;

		private readonly IStyleProcessor m_styleProcessor;

		private readonly ITemplateProcessor m_templateProcessor;

		private readonly ILogger m_logger;

		public ComponentFileProcessor(IScriptProcessor scriptProcessor , IStyleProcessor styleProcessor , ITemplateProcessor templateProcessor , ILogger logger) {
			m_scriptProcessor = scriptProcessor ?? throw new ArgumentNullException(nameof(scriptProcessor));
			m_styleProcessor = styleProcessor ?? throw new ArgumentNullException(nameof(styleProcessor));
			m_templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
			m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc cref="IFileProcessor.Process(string, string, string)"/>
		public void Process(string fileName , string sourceFolder , string targetFolder) {
			var directory = Path.GetDirectoryName(fileName);
			if ( directory == null ) return;

			Directory.CreateDirectory(Path.Combine(targetFolder , directory));
			using var file = File.Open(Path.Combine(sourceFolder , fileName) , FileMode.Open , FileAccess.Read , FileShare.Read);
			using var reader = new StreamReader(file);
			var content = reader.ReadToEnd();

			var componentNamespaceMatch = Regex.Match(content , @"\<template \@.*\>" , RegexOptions.IgnoreCase);
			var componentNamespace = "";
			if ( componentNamespaceMatch != null && !string.IsNullOrEmpty(componentNamespaceMatch.Value) ) {
				componentNamespace = componentNamespaceMatch.Value
					.Replace("<template" , "")
					.Replace(">" , "")
					.Replace("@" , "")
					.Trim();
			}

			var context = GetContext(Path.GetFileName(fileName) , componentNamespace);

			var processedComponent = new StringBuilder();
			processedComponent.Append(GetTemplate(content , context));
			processedComponent.Append(GetScript(content , context));
			processedComponent.Append(GetStyle(content , context));

			File.WriteAllText(Path.Combine(targetFolder , fileName) , processedComponent.ToString());
		}

		private ComponentContextModel GetContext(string fileName , string componentNamespace) {
			return new ComponentContextModel {
				ComponentNamespace = componentNamespace ,
				FileName = fileName
			};
		}

		private string GetScript(string content , ComponentContextModel contextModel) {
			var (startIndex, endIndex) = GetContentPart(content , "script");

			if ( startIndex == -1 ) {
				var errorMessage = $"Component `{contextModel.FileName}` don't have script tag! Script tag is required part of component!";
				m_logger.Log(errorMessage);
				throw new Exception(errorMessage);
			}

			return m_scriptProcessor.ProcessScript(content.Substring(startIndex , endIndex - startIndex).ToString() , contextModel);
		}

		private string GetStyle(string content , ComponentContextModel contextModel) {
			var (startIndex, endIndex) = GetContentPart(content , "style");

			if ( startIndex == -1 ) return "";

			return m_styleProcessor.ProcessStyle(content.Substring(startIndex , endIndex - startIndex).ToString() , contextModel);
		}

		private string GetTemplate(string content , ComponentContextModel contextModel) {
			var (startIndex, endIndex) = GetContentPart(content , "template", withEnding: false);

			if ( startIndex == -1 ) {
				var errorMessage = $"Component `{contextModel.FileName}` don't have template tag! Template tag is required part of component!";
				m_logger.Log(errorMessage);
				throw new Exception(errorMessage);
			}

			return m_templateProcessor.ProcessTemplate(content.Substring(startIndex , endIndex - startIndex).ToString() , contextModel);
		}

		private (int startIndex, int endIndex) GetContentPart(string content , string tag , bool withEnding = true) {
			var closeTag = $"</{tag}>";
			var startIndex = content.IndexOf(withEnding ? $"<{tag}>" : $"<{tag}" , StringComparison.OrdinalIgnoreCase);
			var endIndex = content.LastIndexOf(closeTag , StringComparison.OrdinalIgnoreCase) + closeTag.Length;

			return (startIndex, endIndex);
		}

	}

}
