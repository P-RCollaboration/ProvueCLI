using ProvueCLI.Processors.PresentationClasses;
using System.Threading.Tasks;

namespace ProvueCLI.Processors {

	/// <summary>
	/// Common interface for all template processors.
	/// </summary>
	public interface ITemplateProcessor {

		/// <summary>
		/// Process template tag.
		/// </summary>
		/// <param name="componentContextModel">Model contains component related data.</param>
		/// <param name="content">Template tag in string.</param>
		/// <returns>Precessed template tag.</returns>
		Task<string> ProcessTemplate ( string content , ComponentContextModel componentContextModel );

	}

}
