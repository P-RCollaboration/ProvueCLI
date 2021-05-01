using FakeItEasy;
using ProvueCLI.Processors;
using ProvueCLI.Processors.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProvueCLITests {
	public class FileProcessorFactoryUnitTests {

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void Constructor_Throw_ServiceProviderIsNull () {
			// assert
			Assert.Throws<ArgumentNullException> (
				() => {
					// action
					var factory = new FileProcessorFactory ( null );
				}
			);
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Throw_ExtensionIsNull () {
			// arrange
			var factory = new FileProcessorFactory ( A.Fake<IServiceProvider> () );

			// assert
			Assert.Throws<ArgumentNullException> (
				() => {
					// action
					factory.CreateFileProcessorByExtension ( null );
				}
			);
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Unknown_Extension () {
			// arrange
			var factory = new FileProcessorFactory ( A.Fake<IServiceProvider> () );

			// action
			var fileProcessor = factory.CreateFileProcessorByExtension ( ".unknown" );

			// assert
			Assert.Null ( fileProcessor );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Vue_Extension () {
			// arrange
			var serviceProvider = A.Fake<IServiceProvider> ();
			var factory = new FileProcessorFactory ( serviceProvider );
			A.CallTo ( () => serviceProvider.GetService ( typeof ( IComponentFileProcessor ) ) ).Returns ( A.Fake<ComponentFileProcessor> () );

			// action
			var fileProcessor = factory.CreateFileProcessorByExtension ( ".vue" );

			// assert
			Assert.True( fileProcessor is ComponentFileProcessor );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Html_Extension () {
			// arrange
			var serviceProvider = A.Fake<IServiceProvider> ();
			var factory = new FileProcessorFactory ( serviceProvider );
			A.CallTo ( () => serviceProvider.GetService ( typeof ( IHtmlFileProcessor ) ) ).Returns ( A.Fake<HtmlFileProcessor> () );

			// action
			var fileProcessor = factory.CreateFileProcessorByExtension ( ".html" );

			// assert
			Assert.True ( fileProcessor is HtmlFileProcessor );
		}

	}

}
