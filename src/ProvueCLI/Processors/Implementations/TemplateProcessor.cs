using AngleSharp;
using AngleSharpConfiguration = AngleSharp.Configuration;
using NUglify;
using ProvueCLI.Processors.PresentationClasses;
using System.Threading.Tasks;
using System.Linq;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="ITemplateProcessor"/>
	public class TemplateProcessor : ITemplateProcessor {

		/// <inheritdoc cref="ITemplateProcessor.ProcessTemplate(string, ComponentContextModel)"/>
		public async Task<string> ProcessTemplate(string content , ComponentContextModel componentContextModel) {

			if ( !string.IsNullOrEmpty(componentContextModel.ComponentNamespace) ) {
				content = await AddSuffixToClassesAsync(content , componentContextModel.ComponentNamespace , componentContextModel.FileName);
			}

			if ( Program.ApplicationConfiguration.BuildForRelease ) return Uglify.Html(content).Code;

			return content;
		}

		private async Task<string> AddSuffixToClassesAsync(string content , string componentNamespace , string fileName) {
			var context = BrowsingContext.New(AngleSharpConfiguration.Default);

			// replace tag template to ptemplate because html has own tag template
			var document = await context.OpenAsync(req => req.Content(content.Replace("<template" , "<ptemplate").Replace("</template>" , "</ptemplate>")));

			var templateElement = document.QuerySelector("ptemplate");
			var classesElements = templateElement.QuerySelectorAll("*").Where(a => a.ClassList.Any());
			var pathHash = "-" + ProcessorHelpers.GetPathHash(componentNamespace + fileName);

			foreach ( var elementWithClasses in classesElements ) {
				var classList = elementWithClasses.ClassList.ToList();
				var originalClassList = elementWithClasses.ClassList;
				foreach ( var classItem in classList ) {
					originalClassList.Remove(classItem);
					originalClassList.Add(classItem + pathHash);
				}
			}

			return templateElement.OuterHtml.Replace("<ptemplate" , "<template").Replace("</ptemplate>" , "</template>");
		}
	}

}
