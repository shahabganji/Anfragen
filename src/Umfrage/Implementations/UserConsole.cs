using Umfrage.Abstractions;
using System;
using System.IO;

namespace Umfrage.Implementations {

    public class UserTerminal : IUserTerminal {

        public TextWriter Printer { get; }
        public TextReader Scanner { get; }
        public ConsoleColor ForegroundColor { get { return Console.ForegroundColor; } set { Console.ForegroundColor = value; } }
        public ConsoleColor BackgroundColor { get { return Console.BackgroundColor; } set { Console.BackgroundColor = value; } }

        public UserTerminal( ) {
            this.Printer = Console.Out;
            this.Scanner = Console.In;
        }

        public void ResetColor( ) {
            Console.ResetColor( );
        }
    }
}
