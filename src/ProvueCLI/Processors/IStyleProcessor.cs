using ProvueCLI.Processors.PresentationClasses;
using System.Threading.Tasks;

namespace ProvueCLI.Processors {

	/// <summary>
	/// Common interface for all style processors.
	/// </summary>
	public interface IStyleProcessor {

		/// <summary>
		/// Process style tag.
		/// </summary>
		/// <param name="componentContextModel">Model cantains component related data.</param>
		/// <param name="content">Style tag in string.</param>
		/// <returns>Precessed style tag.</returns>
		Task<string> ProcessStyle ( string content , ComponentContextModel componentContextModel );

	}

}
