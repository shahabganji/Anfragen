using System;
using System.Collections.Generic;
using System.Linq;
using Anfragen.Interfaces;
using System.Xml.Linq;

namespace Anfragen.Implementations {

    public class Confirm : IConfirm {

        public string Question { get; }

        public string Answer { get; private set; }
        public string Hint { get; private set; } = "Yes/No";
        public IList<string> PossibleAnswers { get; }

        public QuestionStates State { get; private set; } = QuestionStates.Initilaized;

        public Confirm( string question, string[ ] possibleAnswers = null ) {
            this.Question = question;
            this.PossibleAnswers = possibleAnswers ?? new[ ] { "Yes", "No" };
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
            var result = validator != null ? validator( this ) : this.PossibleAnswers.Contains( this.Answer );

            this.State = result ? QuestionStates.Valid : QuestionStates.NotValid;

            return this;

        }

        public IQuestion PrintValidationErrors( ) {
            Console.WriteLine( "Your answer must be either 'y' or 'n' ." );
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