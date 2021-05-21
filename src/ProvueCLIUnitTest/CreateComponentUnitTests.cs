using FakeItEasy;
using ProvueCLI.Commands;
using ProvueCLI.FileServices;
using ProvueCLI.PresentationClasses.Commands;
using System.Threading.Tasks;
using Xunit;

namespace ProvueCLITests
{
    public class CreateComponentUnitTests
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task CreateCommand_Positive()
        {
            // arrange
            var args = new string[] { "create", @"examples\helloworld\Test" };
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"C:\Users\Pavel\source\repos\test\CoreComponents\src\examples\helloworld\Test.vue");
            var fileService = A.Fake<IFileService>();

            //var createComponentCommand = new CreateComponent(fileService);

            var createComponentCommand = A.Fake<CreateComponent>();//CreateCommandConfiguration
            //A.CallTo(createComponentCommand)
            //    .Where(x => x.Method.Name == "GetAnswersFromConsole")
            //    .WithReturnType<CreateCommandConfiguration>()
            //    .Returns(new CreateCommandConfiguration());
            //A.CallTo(() => createComponentCommand.GetAnswersFromConsole()).Returns(new CreateCommandConfiguration());


            A.CallTo(() => fileService.GetCurrentDirectory()).Returns("");
            A.CallTo(() => fileService.FileExists("")).Returns(false);
            A.CallTo(() => fileService.GetFileName("")).Returns("Test");
            A.CallTo(() => fileService.WriteToFile("", "")).Returns(Task.CompletedTask);
            var t = createComponentCommand.GetAnswersFromConsole();
            // act
            await createComponentCommand.Execute(args);

            // assert
            A.CallTo(() => fileService.WriteToFile("", "")).MustHaveHappened();
        }

    }
}
