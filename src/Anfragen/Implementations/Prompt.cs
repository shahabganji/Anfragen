using System;
using Anfragen;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {

    public class Prompt : IQuestion {

        public Prompt( string prompt ) {
            this.Question = prompt;
        }

        public string Answer { get; private set; }
        public string Question { get; }

        public string QuestionIcon => "??";

        public void Ask( IPrinter printer ) {
            printer.Print( this.QuestionIcon );
            printer.Print( " " );
            printer.Print( this.Question );
        }

        public void TakeAnswer( ) {
            this.Answer = Console.ReadLine( );
        }

        public bool ValidateAnswer( Func<IQuestion, bool> validator = null ) {

            return validator != null ? validator( this ) : true;
        }

        public void PrintValidationErrors( ) {
            Console.WriteLine( "There are some validation erros" );
        }

        public void Finish( ) {
            var width = Console.WindowWidth;
            while ( width-- > 0 ) {
                Console.Write( "-" );
            }
        }
    }
}