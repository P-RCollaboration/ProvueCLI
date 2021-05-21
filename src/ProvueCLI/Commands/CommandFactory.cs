using System;

namespace ProvueCLI.Commands {
	public class CommandFactory : ICommandFactory {
		private readonly IServiceProvider m_serviceProvider;

		public CommandFactory(IServiceProvider serviceProvider) {
			m_serviceProvider = serviceProvider;
		}

		public ICommand GetCommand(string[] arguments) {
			ICommand command = CreateInstance<ShowHelp>(arguments);

			if ( arguments.Length == 2 && string.CompareOrdinal(arguments[0], "create") == 0 ) {
				command = CreateInstance<CreateComponent>(arguments);
			}

			if ( string.CompareOrdinal(arguments[0], "help") == 0 ) {
				command = CreateInstance<ShowHelp>(arguments);
			}

			return command;
		}

		private ICommand CreateInstance<T>(string[] args) where T : ICommand =>
			(ICommand) ( m_serviceProvider.GetService(typeof(CreateComponent)) ?? throw new Exception($"Command Factory DI exception for {nameof(T)}. arguments:{string.Join(',', args)}") );
	}
}
