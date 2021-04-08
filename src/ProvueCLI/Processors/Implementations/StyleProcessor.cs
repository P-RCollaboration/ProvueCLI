using NUglify;
using ProvueCLI.Loggers;
using ProvueCLI.Processors.PresentationClasses;
using SharpScss;
using System;
using System.Threading.Tasks;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IStyleProcessor"/>
	public class StyleProcessor : IStyleProcessor {

		private readonly ILogger m_logger;

		public StyleProcessor ( ILogger logger ) {
			m_logger = logger ?? throw new ArgumentNullException ( nameof ( logger ) );
		}

		/// <inheritdoc cref="IStyleProcessor.ProcessStyle(string, ComponentContextModel)"/>
		public Task<string> ProcessStyle ( string content , ComponentContextModel componentContextModel ) {
			if ( Program.ApplicationConfiguration.BuildForRelease ) return Task.FromResult ( Uglify.Css ( content ).Code );

			var cssOnly = content.Replace ( "<style>" , "" ).Replace ( "</style>" , "" );

			try {
				var rawCss = Scss.ConvertToCss ( cssOnly ).Css;
				return Task.FromResult ( $"<style>{rawCss}</style>" );
			} catch ( ScssException e ) {
				m_logger.Log ( $"Scss error: {e.Message}" );
				throw;
			}
		}

	}

}
