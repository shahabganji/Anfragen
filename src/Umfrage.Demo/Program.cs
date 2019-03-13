using System;

using Umfrage.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;

namespace Umfrage.Demo
{
	internal class Program
	{
		public static void Main(string[ ] args) {

			//UserConsole terminal = new UserConsole( );
			IQuestionnaire questionnaire = new Questionnaire( );
			questionnaire.Settings.WelcomeMessage = "Welcome to my questionnaire";

			bool Validator(IQuestion x) => x.Answer.Length > 0;
			string errorMessage = "Please Provide a value";


			var builder = questionnaire.Builder;

			IQuestion askName = builder.Simple( ).Text( "What's your name?" ).Build( );

			IQuestion askFamily = builder.Simple( ).Text( "What's your family?" ).AddValidation( Validator, errorMessage ).Build( );

			builder.List()
				.Text("What's your favorite language?")
				.AddOption(new QuestionOption("Persian"))
				.AddOptions(new[]
				{
					new QuestionOption("English"),
					new QuestionOption("Italian"),
					new QuestionOption("Spanish"),
					new QuestionOption("French"),
					new QuestionOption("German")
				})
				.WithHint("Persian")
				.WithDefaultAnswer("Persian")
				.AddValidation(x => x.Answer != null)
				.AsCheckList()
				.WithErrorMessage("You must select an option")
				.AddToQuestionnaire(questionnaire);

            questionnaire
                .Add(askName)
                .Add(askFamily)
                ;

			questionnaire.Start();

			bool add = true;

			// loop until there is a question to ask
			while (questionnaire.CanProceed) {

				if (add) {

					IQuestion confirm = builder.Simple( )
										.Text( "Are you older than 18?" )
										.AsConfirm( )
										.WithHint( "Y/n" )
										.WithDefaultAnswer( "y" )
										.AddValidation( x => {
											Confirm q = ( Confirm ) x;
											return q.PossibleAnswers.Contains( x.Answer );
										}, "Your value should be either 'y' or 'n'" )
										.Build( );

					confirm.Finish((q) => {
						if (q.Answer == "y") {
							IQuestion agePrompt = builder.Simple( )
													.Text( "How old are you?" )
													.AddValidation( x => {

														int.TryParse( x.Answer, out int age );

														return age >= 18;

													}, "Your must be older than 18" )
													.Build( );

                            questionnaire.Prompt(agePrompt as Prompt );
                        }
                    } );

					questionnaire.Confirm(confirm as Confirm);

					add = false;
				}

				questionnaire.Next();

			}

			questionnaire.End();

            // Print Processed questions
            foreach ( var q in questionnaire.ProcessedQuestions ) {

                questionnaire.Terminal.Printer.Write( $"{q.Text} : {q.Answer}" );
                questionnaire.Terminal.Printer.WriteLine( );

			}

			Console.ReadLine();
		}
	}
}
