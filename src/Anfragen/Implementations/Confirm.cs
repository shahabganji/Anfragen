using System;
using System.Collections.Generic;
using System.Linq;
using Anfragen.Interfaces;
using System.Xml.Linq;

namespace Anfragen.Implementations {

    public class Confirm : IConfirm {

        public string Question { get; }

        public IList<string> PossibleAnswers { get; }

        public string Answer { get; private set; }

        public string QuestionIcon => "?";

        //object IQuestion.Answer => throw new NotImplementedException( );

        public Confirm( string question, string[ ] possibleAnswers ) {
            this.Question = question;
            this.PossibleAnswers = possibleAnswers;
        }

        public void Ask( IPrinter printer ) {
            printer.Print( this.Question );
        }

        public void TakeAnswer( ) {
            this.Answer = Console.ReadLine( );
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