using System;
using Microsoft.Extensions.DependencyInjection;

namespace ProvueCLI.Processors.Implementations {

	/// <inheritdoc cref="IFileProcessorFactory"/>
	public class FileProcessorFactory : IFileProcessorFactory {
		
		private readonly IServiceProvider m_serviceProvider;

		public FileProcessorFactory (IServiceProvider serviceProvider) => m_serviceProvider = serviceProvider;

		/// <inheritdoc cref="IFileProcessorFactory.CreateFileProcessorByExtension(string)"/>
		public IFileProcessor? CreateFileProcessorByExtension ( string extension ) {
			IFileProcessor? processor = null;
			switch ( extension ) {
				case ".vue":
					processor = m_serviceProvider.GetService<IComponentFileProcessor> ();
					break;
				case ".html":
					processor = m_serviceProvider.GetService<IHtmlFileProcessor> ();
					break;
				case ".js":
					processor = m_serviceProvider.GetService<IScriptFileProcessor> ();
					break;
				case ".css":
					processor = m_serviceProvider.GetService<IStyleFileProcessor> ();
					break;
				case ".png":
				case ".jpg":
				case ".jpeg":
				case ".gif":
				case ".svg":
				case ".webp":
					processor = m_serviceProvider.GetService<IImageFileProcessor> ();
					break;
			}

			return processor;
		}

	}

}
