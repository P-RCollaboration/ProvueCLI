using System.IO;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	public class StyleFileProcessor : IStyleFileProcessor {

		public void Process ( string fileName , string sourceFolder , string targetFolder ) {
			var directory = Path.GetDirectoryName ( fileName );
			if ( directory == null ) return;

			Directory.CreateDirectory ( Path.Combine ( targetFolder , directory ) );

			File.Copy ( Path.Combine ( sourceFolder , fileName ) , Path.Combine ( targetFolder , fileName ) , overwrite: true );
		}

	}

}
