namespace ProvueCLI.Processors.PresentationClasses {

	/// <summary>
	/// Component context data.
	/// </summary>
	public record ComponentContextModel {

		/// <summary>
		/// Component identifier.
		/// </summary>
		public string ComponentId { get; set; } = "";

		/// <summary>
		/// File name.
		/// </summary>
		public string FileName { get; set; } = "";

		/// <summary>
		/// Component namespace.
		/// </summary>
		public string ComponentNamespace { get; set; } = "";

	}

}
