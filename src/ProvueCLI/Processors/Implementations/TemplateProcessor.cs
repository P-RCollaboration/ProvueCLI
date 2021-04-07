using NUglify;
using ProvueCLI.Processors.PresentationClasses;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="ITemplateProcessor"/>
	public class TemplateProcessor : ITemplateProcessor {

		/// <inheritdoc cref="ITemplateProcessor.ProcessTemplate(string, ComponentContextModel)"/>
		public Task<string> ProcessTemplate ( string content , ComponentContextModel componentContextModel ) {
			if ( Program.ApplicationConfiguration.BuildForRelease ) return Task.FromResult ( Uglify.Html ( content ).Code );

			return Task.FromResult ( content );
		}

	}

}
