using System;
using System.Collections.Generic;
using System.Linq;
using Anfragen.Interfaces;
using System.Xml.Linq;

namespace Anfragen.Implementations {

    public class Confirm : IConfirm {

        public string Question { get; }
        public string QuestionIcon => "?";

        public string Answer { get; private set; }
        public IList<string> PossibleAnswers { get; }

        public Confirm( string question, string[ ] possibleAnswers ) {
            this.Question = question;
            this.PossibleAnswers = possibleAnswers;
        }

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

        public bool ValidateAnswer( Func<IQuestion, bool> validator = null ) {
            return validator != null ? validator( this ) : this.PossibleAnswers.Contains( this.Answer );
        }

        public void PrintValidationErrors( ) {
            Console.WriteLine( "Your answer must be either 'y' or 'n' ." );
        }

        public void Finish( ) {
            var width = Console.WindowWidth;
            while ( width-- > 0 ) {
                Console.Write( "-" );
            }
        }

    }

}