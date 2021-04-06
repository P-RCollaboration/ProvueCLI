using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <summary>
	/// Process .vue files.
	/// </summary>
	public class ComponentProcessor : IFileProcessor {

		/// <inheritdoc cref="IFileProcessor.Process(string, string, string)"/>
		public Task Process ( string fileName , string sourceFolder , string targetFolder ) {
			var directory = Path.GetDirectoryName ( fileName );
			if ( directory == null ) return Task.CompletedTask;

			Directory.CreateDirectory ( Path.Combine ( targetFolder , directory ) );
			var content = Path.Combine ( sourceFolder , fileName );
			var targetFileName = Path.Combine ( targetFolder , fileName );

			return Task.CompletedTask;
		}

	}

}
