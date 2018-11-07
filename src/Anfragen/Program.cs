using System;
using System.Runtime.CompilerServices;
using Anfragen.Implementations;
using Anfragen.Interfaces;

namespace Anfragen {

    public class Program {

        public static void Main( string[ ] args ) {

            var writer = new ConsoleWriter( );
            var questionnaire = new Questionnaire( writer );

            IQuestion ask_name = new Prompt( "What's your name? " );

            IQuestion confrim_health = new Confirm( "Are you okay ?", new[ ] { "1", "2" } );

            questionnaire.Add( ask_name )
                         .Add( confrim_health );

            questionnaire.Start( );

            if ( !questionnaire.CurrentQuestion.ValidateAnswer( ) ) {
                questionnaire.CurrentQuestion.PrintValidationErrors( );
            }

            questionnaire.CurrentQuestion.EndTheQuestion( );
            questionnaire.Add( new Confirm(
                                            $"Are you {questionnaire.CurrentQuestion.Answer}? ",
                                             new[ ] { "yes", "no" } ), true );

            questionnaire.Next( );
            questionnaire.Next( );

        }
    }
}
