namespace ProvueCLI.Processors {

	/// <summary>
	/// Helpers for processors.
	/// </summary>
	public static class ProcessorHelpers {

		/// <summary>
		/// Get hash from path.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <returns>Computed hash.</returns>
		public static string GetPathHash(string path) {
			int hash = 0;

			int chr;
			for ( var i = 0 ; i < path.Length ; i++ ) {
				chr = path[i].GetHashCode();
				hash = ( ( hash << 5 ) - hash ) + chr;
				hash |= 0;
			}
			return hash.ToString().Replace("-" , "");
		}

	}

}
