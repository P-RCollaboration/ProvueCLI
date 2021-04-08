using System.Threading.Tasks;

namespace ProvueCLI.Processors {

	/// <summary>
	/// Folder processor.
	/// </summary>
	public interface IFolderProcessor {

		/// <summary>
		/// Process all related files in folder.
		/// </summary>
		/// <param name="sourceFolder">The folder that contains original files.</param>
		/// <param name="targetFolder">The folder where will be generate processed files.</param>
		Task ProcessFiles ( string sourceFolder , string targetFolder );

	}

}
