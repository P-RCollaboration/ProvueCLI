using System;

namespace ProvueCLI.Loggers.Implementations {

	/// <summary>
	/// Logger for console.
	/// </summary>
	public class ConsoleLogger : ILogger {

		public void Log ( string message ) {
			Console.WriteLine ( message );
		}

	}

}
