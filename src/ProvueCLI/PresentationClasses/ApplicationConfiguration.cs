namespace ProvueCLI.PresentationClasses {

	/// <summary>
	/// Class contains all applciation configuration.
	/// </summary>
	public record ApplicationConfiguration {

		public string SourceFolder { get; set; } = "";

		public string BuildFolder { get; set; } = "";

		public string WebServerFolder { get; set; } = "";

		public bool IsRunDeveloplementServer { get; set; } = false;

		public int WebServerPort { get; set; } = 8080;

		public string WebServerHost { get; set; } = "localhost";

		public string ReleaseFolder { get; set; } = "";

		public bool BuildForRelease { get; set; } = false;

		public bool IsEmpty () {
			return string.IsNullOrEmpty ( SourceFolder ) &&
				string.IsNullOrEmpty ( BuildFolder ) &&
				string.IsNullOrEmpty ( WebServerFolder ) &&
				string.IsNullOrEmpty ( ReleaseFolder );
		}

	}

}
