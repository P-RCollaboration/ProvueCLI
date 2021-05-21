using System.Threading.Tasks;

namespace ProvueCLI.Commands {
	public interface ICommand {
		Task Execute(string[] args);
	}
}
