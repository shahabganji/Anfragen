using System;
using System.Runtime.CompilerServices;
using Anfragen.Implementations;
using Anfragen.Interfaces;

namespace Anfragen {

    public class Program {

        public static void Main( string[ ] args ) {

            var writer = new ConsoleWriter( );
            IQuestionnaire questionnaire = new Questionnaire( writer );

            IQuestion ask_name = new Prompt( "What's your name? " );

            IQuestion confrim_health = new Confirm( "Are you okay? ", new[ ] { "1", "2" } );

            IBranch health_branch = new Branch( "health" );
            health_branch.Add( confrim_health );

            questionnaire.Add( ask_name )
                         .Add( health_branch );


            questionnaire.Start( );

            if ( questionnaire.CurrentQuestion.ValidateAnswer( ).State == QuestionStates.NotValid ) {
                questionnaire.CurrentQuestion.PrintValidationErrors( );
            }
            questionnaire.CurrentQuestion.Finish( );

            // Dynamic question adding to the main branch
            questionnaire.Add( new Confirm(
                                            $"Are you {questionnaire.CurrentQuestion.Answer}? ",
                                            new[ ] { "yes", "no" } ), here: true
                             );

            questionnaire.GoToNextStep( ).CurrentQuestion.Finish( );


            questionnaire.GoToBranch( "health" );

            var isValid = questionnaire.GoToNextStep( ).CurrentQuestion.ValidateAnswer( q => {
                return q.Answer.Length > 1;
            } ).State;

            if ( isValid == QuestionStates.NotValid ) {
                questionnaire.CurrentQuestion.PrintValidationErrors( );
                questionnaire.CurrentQuestion.Ask( writer ).TakeAnswer( );
            }
            questionnaire.CurrentQuestion.Finish( );

            questionnaire.End( );

        }
    }
}
