using AngleSharp;
using AngleSharp.Css.Dom;
using NUglify;
using ProvueCLI.Loggers;
using ProvueCLI.Processors.PresentationClasses;
using SharpScss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharpConfiguration = AngleSharp.Configuration;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IStyleProcessor"/>
	public class StyleProcessor : IStyleProcessor {

		private readonly ILogger m_logger;

		public StyleProcessor(ILogger logger) {
			m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc cref="IStyleProcessor.ProcessStyle(string, ComponentContextModel)"/>
		public async Task<string> ProcessStyle(string content , ComponentContextModel componentContextModel) {
			var pureCss = ConvertFromScss(content);

			if ( Program.ApplicationConfiguration.BuildForRelease ) return Uglify.Css(pureCss).Code;

			if ( !string.IsNullOrEmpty(componentContextModel.ComponentNamespace) ) {
				pureCss = await AddSuffixToClassesAsync(pureCss , componentContextModel.ComponentNamespace , componentContextModel.FileName);
			}

			return pureCss;
		}

		private string ConvertFromScss(string content) {
			var cssOnly = content.Replace("<style>" , "").Replace("</style>" , "");

			try {
				var rawCss = Scss.ConvertToCss(cssOnly).Css;
				return $"<style>{rawCss}</style>";
			} catch ( ScssException e ) {
				m_logger.Log($"Scss error: {e.Message}");
				throw;
			}
		}

		private async Task<string> AddSuffixToClassesAsync(string content , string componentNamespace , string fileName) {
			var context = BrowsingContext.New(AngleSharpConfiguration.Default.WithCss());

			// replace tag template to ptemplate because html has own tag template
			var document = await context.OpenAsync(req => req.Content(content));
			var sheets = document.StyleSheets.ToList().Cast<ICssStyleSheet>();
			var styleElement = document.QuerySelector("style");
			var pathHash = "-" + ProcessorHelpers.GetPathHash(componentNamespace + fileName);

			var classes = new HashSet<string>();
			foreach ( var sheet in sheets ) {
				var rules = sheet.Rules.Cast<ICssStyleRule>();
				foreach ( var rule in rules ) {
					var selectorText = rule.SelectorText;
					var matches = Regex.Matches(selectorText , @"\.[_a-zA-Z0-9-]{0,}").ToList().Select(a => a.Value).Where(a => !string.IsNullOrEmpty(a) && a != ".");
					foreach ( var match in matches ) {
						classes.Add(match);
					}
				}
			}

			foreach ( var classItem in classes ) {
				content = content.Replace(classItem , classItem + pathHash);
			}

			return content;
		}
	}

}
