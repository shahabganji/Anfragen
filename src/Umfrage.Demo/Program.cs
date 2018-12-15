using System;
using Umfrage.Abstractions;
using Umfrage.Builders;
using Umfrage.Builders.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;

namespace Umfrage.Demo {
    internal class Program {
        public static void Main( string[ ] args ) {

            UserConsole terminal = new UserConsole( );
            IQuestionnaire questionnaire = new Questionnaire( terminal );
            questionnaire.Settings.WelcomeMessage = "Welcome to my questionnaire";

            Func<IQuestion, bool> validator = x => x.Answer.Length > 0;
            string errorMessage = "Please Provide a value";

            IQuestionBuilder builder = new QuestionBuilder( );

            IQuestion ask_name = builder.Simple( ).New( "What's your name?" ).Build( );

            IQuestion ask_family = builder.Simple( ).New( "What's your family?" ).AddValidation( validator, errorMessage ).Build( );

            IQuestion preferable_language = builder.List( )
                                            .New( "What's your favorite language?" )
                                            .AddOption( new QuestionOption( "Persian" ) )
                                            .AddOptions( new[ ] {
                                                new QuestionOption("English"),
                                                new QuestionOption("Italian"),
                                                new QuestionOption("Spanish"),
                                                new QuestionOption("French"),
                                                new QuestionOption("German")
                                            } )
                                            .AddValidation( x => x.Answer != null )
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

                    IQuestion confirm = builder.Simple( )
                                        .New( "Are you older than 18?" )
                                        .AsConfirm( )
                                        .WithHint( "y/n" )
                                        .AddValidation( x => {
                                            Confirm q = ( Confirm ) x;
                                            return q.PossibleAnswers.Contains( x.Answer );
                                        }, "Your value should be either 'y' or 'n'" )
                                        .Build( );

                    confirm.Finish( ( q ) => {
                        if ( q.Answer == "y" ) {
                            IQuestion age_prompt = builder.Simple( )
                                                    .New( "How old are you?" )
                                                    .AddValidation( x => {

                                                        int.TryParse( x.Answer, out int age );

                                                        return age >= 18;

                                                    }, "Your must be older than 18" )
                                                    .Build( );

                            questionnaire.Prompt( age_prompt as Prompt );
                        }
                    } );

                    questionnaire.Confirm( confirm as Confirm );

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
