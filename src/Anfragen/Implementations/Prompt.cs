using System;
using Anfragen;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {

    public class Prompt : IPrompt {

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

        int cursorTop, cursorLeft;

        public IQuestion Ask( IPrinter printer ) {
            printer.Print( " " );
            printer.Print( this.Question );
            this.State = QuestionStates.Asked;

            return this;
        }

        public virtual IQuestion TakeAnswer( ) {

            if ( this.State == QuestionStates.NotValid ) {
                Console.SetCursorPosition( left: this.cursorLeft, top: this.cursorTop );
            }

            this.cursorTop = Console.CursorTop;
            this.cursorLeft = Console.CursorLeft;

            this.Answer = Console.ReadLine( );
            this.State = QuestionStates.Answered;

            return this;
        }

        public IQuestion Validate( Func<IQuestion, bool> validator = null ) {

            var result = validator != null ? validator( this ) : this.Answer.Trim( ).Length > 0;
            this.State = result ? QuestionStates.Valid : QuestionStates.NotValid;

            if ( result ) {
                this.ClearConsoleLine( this.cursorTop + 1 );
            }

            return this;
        }

        public IQuestion PrintValidationErrors( ) {
            Console.WriteLine( "Please enter a value for the current prompt." );
            return this;
        }

        public void Finish( Action done = null ) {

            if ( this.State == QuestionStates.NotValid ) {
                throw new InvalidOperationException( "You cannot finish a question while it is in an invalid state." );
            }

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