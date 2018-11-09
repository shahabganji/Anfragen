using System;
using Anfragen;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {

    public class Prompt : IQuestion {

        #region properties

        public string Answer { get; private set; }
        public string Question { get; }
        public string Hint { get; }

        public QuestionStates State { get; private set; } = QuestionStates.Initilaized;

        #endregion


        public Prompt( string prompt, string hint = "" ) {
            this.Question = prompt;
            this.Hint = hint;
        }

        public IQuestion Ask( IPrinter printer ) {
            printer.Print( " " );
            printer.Print( this.Question );
            this.State = QuestionStates.Asked;
            return this;
        }

        public IQuestion TakeAnswer( ) {
            this.Answer = Console.ReadLine( );
            this.State = QuestionStates.Answered;
            return this;
        }

        public IQuestion Validate( Func<IQuestion, bool> validator = null ) {

            var result = validator != null ? validator( this ) : true;
            this.State = result ? QuestionStates.Valid : QuestionStates.NotValid;

            return this;
        }

        public IQuestion PrintValidationErrors( ) {

            Console.WriteLine( "There are some validation erros" );
            return this;
        }

        public void Finish( Action done = null ) {
            var width = Console.WindowWidth;
            while ( width-- > 0 ) {
                Console.Write( "-" );
            }

            done?.Invoke( );

            this.State = QuestionStates.Finished;
        }

        public void PrintResult( IPrinter printer ) {
            printer.Print( $"{this.Question} : {this.Answer}" );
            printer.AddNewLine( );
        }
    }
}