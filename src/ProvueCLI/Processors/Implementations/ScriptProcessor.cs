using NUglify;
using ProvueCLI.Processors.PresentationClasses;

namespace ProvueCLI.Processors.Implementations {

	public class ScriptProcessor : IScriptProcessor {

		public string ProcessScript ( string content , ComponentContextModel componentContextModel ) {

			// minification and ulifying for release
			if ( Program.ApplicationConfiguration.BuildForRelease ) {
				return Uglify.Js ( content ).Code;
			} else {
				content = content.Replace ( "</script>" , $"//# sourceURL={componentContextModel.FileName}\n</script>" );
			}

			return content;
		}

	}

}
