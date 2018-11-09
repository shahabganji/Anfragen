using System;
using Anfragen.Implementations;
using Anfragen.Interfaces;
using Anfragen.Extensions;

namespace Anfragen {

    public class Program {

        public static void Main( string[ ] args ) {

            var writer = new ConsoleWriter( );
            IQuestionnaire questionnaire = new Questionnaire( writer );

            IQuestion ask_name = new Prompt( "What's your name?" );
            IQuestion ask_family = new Prompt( "What's your family?" );

            questionnaire.Add( ask_name ).Add( ask_family );

            questionnaire.Start( );

            QuestionStates states = QuestionStates.NotValid;

            bool add = true;

            // loop until there is a question to ask
            while ( questionnaire.CanProceed ) {

                while ( states == QuestionStates.NotValid || states == QuestionStates.Answered ) {
                    states = questionnaire.Validate( ).CurrentQuestion.State;
                }

                questionnaire.CurrentQuestion.Finish( );

                if ( add ) {
                    questionnaire.Prompt( new Prompt( "How old are you? " ) );
                    add = false;
                }

                questionnaire.GoToNextStep( );
            }


            // for the last question
            states = questionnaire.Validate( ).CurrentQuestion.State;
            while ( states == QuestionStates.NotValid || states == QuestionStates.Answered ) {
                states = questionnaire.Validate( ).CurrentQuestion.State;
            }

            questionnaire.CurrentQuestion.Finish( );

            questionnaire.End( );

            // Print Processed questions
            foreach ( var q in questionnaire.ProcessedQuestions ) {
                writer.Print( $"{q.Question} : {q.Answer}" );
                writer.AddNewLine( );
            }

        }
    }
}
