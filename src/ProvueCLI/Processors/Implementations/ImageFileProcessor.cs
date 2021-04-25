using System.IO;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IImageFileProcessor"/>
	public class ImageFileProcessor : IImageFileProcessor {

		/// <inheritdoc cref="IFileProcessor.Process(string, string, string)" />
		public Task Process ( string fileName , string sourceFolder , string targetFolder ) {
			var directory = Path.GetDirectoryName ( fileName );
			if ( directory == null ) return Task.CompletedTask;

			Directory.CreateDirectory ( Path.Combine ( targetFolder , directory ) );

			return Task.Run (
				() => {
					File.Copy ( Path.Combine ( sourceFolder , fileName ) , Path.Combine ( targetFolder , fileName ) , overwrite: true );
				}
			);
		}

	}

}
