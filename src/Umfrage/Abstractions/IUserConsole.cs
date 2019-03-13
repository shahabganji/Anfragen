using System;
using System.IO;

namespace Umfrage.Abstractions
{

	public interface IUserTerminal {

		TextWriter Printer { get; }
		TextReader Scanner { get; }

		ConsoleColor ForegroundColor { get; set; }
		ConsoleColor BackgroundColor { get; set; }

		void ResetColor();

	}
}
