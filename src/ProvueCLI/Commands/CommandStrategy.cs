using ProvueCLI.Loggers;
using System;
using System.Threading.Tasks;

namespace ProvueCLI.Commands {
	public class CommandStrategy {
		private readonly ILogger m_logger;
		private readonly ICommandFactory m_commandFactory;

		public CommandStrategy(ILogger logger, ICommandFactory commandFactory) {
			m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			m_commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
		}

		public async Task TryExecuteCommand(string[] args) {
			try {
				ICommand command = m_commandFactory.GetCommand(args);

				await command.Execute(args);
			} catch ( Exception ex ) {
				m_logger.Log(ex.ToString());
			}
		}
	}
}
