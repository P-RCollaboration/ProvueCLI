using System.IO;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IHtmlFileProcessor"/>
	public class HtmlFileProcessor : IHtmlFileProcessor {

		/// <inheritdoc cref="IFileProcessor.Process(string, string, string)" />
		public void Process ( string fileName , string sourceFolder , string targetFolder ) {
			var directory = Path.GetDirectoryName ( fileName );
			if ( directory == null ) return;

			Directory.CreateDirectory ( Path.Combine ( targetFolder , directory ) );

			File.Copy ( Path.Combine ( sourceFolder , fileName ) , Path.Combine ( targetFolder , fileName ), overwrite: true );
		}

	}

}
