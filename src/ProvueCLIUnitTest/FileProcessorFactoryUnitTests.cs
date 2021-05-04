using FakeItEasy;
using ProvueCLI.Processors;
using ProvueCLI.Processors.Implementations;
using System;
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
			Assert.True ( fileProcessor is ComponentFileProcessor );
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

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Js_Extension () {
			// arrange
			var serviceProvider = A.Fake<IServiceProvider> ();
			var factory = new FileProcessorFactory ( serviceProvider );
			A.CallTo ( () => serviceProvider.GetService ( typeof ( IScriptFileProcessor ) ) ).Returns ( A.Fake<ScriptFileProcessor> () );

			// action
			var fileProcessor = factory.CreateFileProcessorByExtension ( ".js" );

			// assert
			Assert.True ( fileProcessor is ScriptFileProcessor );
		}

		[Fact]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Css_Extension () {
			// arrange
			var serviceProvider = A.Fake<IServiceProvider> ();
			var factory = new FileProcessorFactory ( serviceProvider );
			A.CallTo ( () => serviceProvider.GetService ( typeof ( IStyleFileProcessor ) ) ).Returns ( A.Fake<StyleFileProcessor> () );

			// action
			var fileProcessor = factory.CreateFileProcessorByExtension ( ".css" );

			// assert
			Assert.True ( fileProcessor is StyleFileProcessor );
		}

		[Theory]
		[InlineData ( ".png" )]
		[InlineData ( ".jpg" )]
		[InlineData ( ".jpeg" )]
		[InlineData ( ".gif" )]
		[InlineData ( ".svg" )]
		[InlineData ( ".webp" )]
		[Trait ( "Category" , "Unit" )]
		public void CreateFileProcessorByExtension_Image_Extension ( string extension ) {
			// arrange
			var serviceProvider = A.Fake<IServiceProvider> ();
			var factory = new FileProcessorFactory ( serviceProvider );
			A.CallTo ( () => serviceProvider.GetService ( typeof ( IImageFileProcessor ) ) ).Returns ( A.Fake<ImageFileProcessor> () );

			// action
			var fileProcessor = factory.CreateFileProcessorByExtension ( extension );

			// assert
			Assert.True ( fileProcessor is ImageFileProcessor );
		}

	}

}
