using System;
using Anfragen;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {

    public class Prompt : IQuestion {

        public Prompt( string prompt ) {
            this.Question = prompt;
        }

        public QuestionStates State { get; private set; } = QuestionStates.Initilaised;

        public string Answer { get; private set; }
        public string Question { get; }

        public string QuestionIcon => "??";

        public IQuestion Ask( IPrinter printer ) {
            printer.Print( this.QuestionIcon );
            printer.Print( " " );
            printer.Print( this.Question );
            return this;
        }

        public IQuestion TakeAnswer( ) {
            this.Answer = Console.ReadLine( );
            return this;
        }

        public IQuestion ValidateAnswer( Func<IQuestion, bool> validator = null ) {

            var result = validator != null ? validator( this ) : true;
            this.State = result ? QuestionStates.Valid : QuestionStates.NotValid;

            return this;
        }

        public IQuestion PrintValidationErrors( ) {

            Console.WriteLine( "There are some validation erros" );
            return this;
        }

        public void Finish( ) {
            var width = Console.WindowWidth;
            while ( width-- > 0 ) {
                Console.Write( "-" );
            }
        }
    }
}