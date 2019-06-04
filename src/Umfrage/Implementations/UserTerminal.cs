using System;
using System.IO;
using Umfrage.Abstractions;

namespace Umfrage.Implementations
{
    public class UserTerminal : IUserTerminal
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

        public UserTerminal()
            : this(Console.Out, Console.In)
        {
        }

        public UserTerminal(TextWriter writer, TextReader reader)
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
