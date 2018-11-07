
using System;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {

    public class ConsoleWriter : IPrinter {

        public void AddNewLine( ) {
            Console.Write( "\n" );
        }

        public void Print( string text ) {
            Console.Write( text );
        }
    }
}
