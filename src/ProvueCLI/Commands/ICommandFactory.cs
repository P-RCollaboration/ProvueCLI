namespace ProvueCLI.Commands {
	public interface ICommandFactory {
		ICommand GetCommand(string[] arguments);
	}
}
