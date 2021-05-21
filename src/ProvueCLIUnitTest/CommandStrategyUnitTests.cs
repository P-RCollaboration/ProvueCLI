using FakeItEasy;
using ProvueCLI.Commands;
using ProvueCLI.Loggers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProvueCLITests
{
    public class CommandStrategyUnitTests {

        [Fact]
        [Trait("Category", "Unit")]
        public async Task CreateCommand_Positive() {
            // arrange
            var args = new string[] { "create", @"examples\helloworld\Test" };
            var logger = A.Fake<ILogger>();
            var createComponentCommand = A.Fake<ICommand>();
            var commandFactory = A.Fake<ICommandFactory>();
            var commandStrategy = new CommandStrategy(logger, commandFactory);

            A.CallTo(() => commandFactory.GetCommand(args)).Returns(createComponentCommand);

            // act
            await commandStrategy.TryExecuteCommand(args);

            // assert
            A.CallTo(() => createComponentCommand.Execute(args)).MustHaveHappened();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task CreateCommand_Throw_FactoryFail()
        {
            // arrange
            var args = new string[] { "create", @"examples\helloworld\Test" };
            var logger = A.Fake<ILogger>();
            var createComponentCommand = A.Fake<ICommand>();
            var commandFactory = A.Fake<ICommandFactory>();
            var commandStrategy = new CommandStrategy(logger, commandFactory);

            A.CallTo(() => commandFactory.GetCommand(args)).Throws(new Exception("TEST"));

            // act
            await commandStrategy.TryExecuteCommand(args);

            // assert
            A.CallTo(() => createComponentCommand.Execute(args)).MustNotHaveHappened();
            A.CallTo(() => logger.Log(A<string>._)).MustHaveHappened();
        }
    }
}
