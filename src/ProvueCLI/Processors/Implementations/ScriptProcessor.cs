using NUglify;
using ProvueCLI.Processors.PresentationClasses;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IScriptProcessor"/>
	public class ScriptProcessor : IScriptProcessor {

		/// <inheritdoc cref="IScriptProcessor.ProcessScript(string, ComponentContextModel)"/>
		public Task<string> ProcessScript ( string content , ComponentContextModel componentContextModel ) {

			// minification and ulifying for release
			if ( Program.ApplicationConfiguration.BuildForRelease ) return Task.FromResult ( Uglify.Js ( content ).Code );

			//TODO: support transpilers like babel

			return Task.FromResult ( content );
		}

	}

}
