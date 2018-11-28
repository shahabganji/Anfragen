using Anfragen.Extensions;
using Anfragen.Implementations;
using Anfragen.Interfaces;
using System;

namespace Anfragen.Demo {
    internal class Program {
        public static void Main( string[ ] args ) {

            UserConsole terminal = new UserConsole( );
            IQuestionnaire questionnaire = new Questionnaire( terminal );
            questionnaire.Settings.WelcomeMessage = "Welcome to my questionnare";

            Func<Question, bool> validator = x => x.Answer.Length > 0;
            string errorMessage = "Please Provide a value";

            Question ask_name = new Prompt( "What's your name?" ).Validator( validator, errorMessage );
            Question ask_family = new Prompt( "What's your family?" ).Validator( validator, errorMessage );

            var builder = new SelectableListQuestionBuilder( );

            Question preferable_language = builder
                                            .New( "What's your favorite language?" )
                                            .AddValidation( x => x.Answer != null )
                                            .AddOption( new QuestionOption( "Persian" ) )
                                            .AddOptions( new[ ] {
                                                new QuestionOption("English"),
                                                new QuestionOption("Italian"),
                                                new QuestionOption("Spanish"),
                                                new QuestionOption("French"),
                                                new QuestionOption("German")
                                            } )
                                            .AsCheckList( )
                                            .AddToQuestionnaire( questionnaire )
                                            .WithErrorMessage( "You must select an option" )
                                            .Build( );

            questionnaire
                .Add( ask_name )
                .Add( ask_family )
                ;

            questionnaire.Start( );

            bool add = true;

            // loop until there is a question to ask
            while ( questionnaire.CanProceed ) {

                if ( add ) {

                    Confirm confirm = new Confirm( "Are you older than 18?" );

                    confirm.Validator( x => {
                        Confirm q = ( Confirm ) x;
                        return q.PossibleAnswers.Contains( x.Answer );
                    }, "Your value should be either 'Yes' or 'No'" );

                    questionnaire.Confirm( confirm );

                    Prompt age_prompt = new Prompt( "How old are you?" );
                    age_prompt.Validator( x => {

                        int age;
                        int.TryParse( x.Answer, out age );

                        return age >= 18;

                    }, "Your must be older than 18" );
                    questionnaire.Prompt( age_prompt );
                    add = false;
                }

                questionnaire.Next( );

            }

            questionnaire.End( );

            // Print Processed questions
            foreach ( Question q in questionnaire.ProcessedQuestions ) {

                terminal.Printer.Write( $"{q.Text} : {q.Answer}" );
                terminal.Printer.WriteLine( );

            }

            Console.ReadLine( );
        }
    }
}
