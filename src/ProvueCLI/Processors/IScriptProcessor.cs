using ProvueCLI.Processors.PresentationClasses;
using System.Threading.Tasks;

namespace ProvueCLI.Processors {

	/// <summary>
	/// Common interface for all script processors.
	/// </summary>
	public interface IScriptProcessor {

		/// <summary>
		/// Process script tag.
		/// </summary>
		/// <param name="componentContextModel">Model cantains component related data.</param>
		/// <param name="content">Script tag in string.</param>
		/// <returns>Precessed script tag.</returns>
		Task<string> ProcessScript ( string content , ComponentContextModel componentContextModel );

	}

}
