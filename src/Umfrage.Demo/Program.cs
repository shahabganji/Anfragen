using System;

using Umfrage.Abstractions;
using Umfrage.Builders;
using Umfrage.Builders.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;

namespace Umfrage.Demo
{
	internal static class Program
	{
		public static void Main(string[ ] args) {

			IQuestionnaire questionnaire = new Questionnaire( );
			questionnaire.Settings.WelcomeMessage = "Welcome to my questionnaire";

			IQuestionBuilder builder = new QuestionBuilder();

			bool Validator(IQuestion x) => x.Answer.Length > 0;
			string errorMessage = "Please Provide a value";


			IQuestion askName = builder.Simple( ).Text( "What's your name?" ).Build( );
			builder.Simple( ).Text( "What's your family?" ).AddValidation( Validator, errorMessage ).AddToQuestionnaire( questionnaire );

			questionnaire.Add(askName);

			builder.List()
				.Text("What's your favorite language?")
				.AddOptions(new[]
				{
					new QuestionOption("Persian"),
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

			questionnaire.Start();

			bool add = true;

			// loop as far as there is a question to ask
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

							var agePrompt = new Prompt("How old are you?");

							agePrompt.Validator(x =>
							{

								int.TryParse(x.Answer, out int age);

								return age >= 18;

							}, "You must be older than 18");

							// Use of Extension methods
							questionnaire.Prompt(agePrompt);
                        }
                    } );

					// Add a builder-made question via an extension method
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
