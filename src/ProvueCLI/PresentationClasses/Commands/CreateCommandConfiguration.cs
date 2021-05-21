using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProvueCLI.PresentationClasses.Commands {
	public record CreateCommandConfiguration {
		public bool IsComponentAsync { get; set; }
		public string? ComponentNamespace { get; set; }
		public bool HasStyleSection { get; set; }

		public readonly Dictionary<string, Func<string, bool>> m_steps;

		private readonly HashSet<string> m_yesAnswerOptions;

		public CreateCommandConfiguration() {
			m_steps = new Dictionary<string, Func<string, bool>> {
				["Create async component? [Y/n] "] = ParseIsComponentAsyncPropery,
				["Type component namespace: "] = ParseComponentNamespacePropery,
				["Add style section? [Y/n] "] = ParseHasStyleSectionPropery
			};
			m_yesAnswerOptions = new HashSet<string> {
				"yes",
				"yep",//=)
				"y"
			};
		}

		public bool ParseIsComponentAsyncPropery(string text) {
			if ( !string.IsNullOrEmpty(text) || m_yesAnswerOptions.Contains(text.ToLowerInvariant())) {
				IsComponentAsync = true;
				return true;
			}

			return false;
		}
		public bool ParseComponentNamespacePropery(string text) {
			if ( !string.IsNullOrEmpty(text) && !Regex.IsMatch(text, "\"(^\"|\"\")*\"") ) {
				ComponentNamespace = text;
				return true;
			}

			return false;
		}

		public bool ParseHasStyleSectionPropery(string text) {
			if ( !string.IsNullOrEmpty(text) || m_yesAnswerOptions.Contains(text.ToLowerInvariant()) ) {
				IsComponentAsync = true;
				return true;
			}

			return false;
		}
	}
}
