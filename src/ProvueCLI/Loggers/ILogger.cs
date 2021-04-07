using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvueCLI.Loggers {
	
	/// <summary>
	/// Interface for logging information.
	/// </summary>
	public interface ILogger {

		void Log ( string message );

	}

}
