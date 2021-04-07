using NUglify;
using ProvueCLI.Processors.PresentationClasses;
using SharpScss;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IStyleProcessor"/>
	public class StyleProcessor : IStyleProcessor {

		/// <inheritdoc cref="IStyleProcessor.ProcessStyle(string, ComponentContextModel)"/>
		public Task<string> ProcessStyle ( string content , ComponentContextModel componentContextModel ) {
			if ( Program.ApplicationConfiguration.BuildForRelease ) return Task.FromResult ( Uglify.Css ( content ).Code );

			return Task.FromResult ( Scss.ConvertToCss ( content ).Css );
		}

	}

}
