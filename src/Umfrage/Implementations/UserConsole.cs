using Umfrage.Abstractions;
using System;
using System.IO;

namespace Umfrage.Implementations
{
    public class UserConsole : IUserTerminal
    {
        public TextWriter Printer { get; }
        public TextReader Scanner { get; }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public UserConsole()
            : this(Console.Out, Console.In)
        {
        }

        public UserConsole(TextWriter writer, TextReader reader)
        {
            this.Printer = writer;
            this.Scanner = reader;
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}
