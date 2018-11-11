using Anfragen.Extensions;
using Anfragen.Implementations;
using Anfragen.Interfaces;
using System;
using System.Linq.Expressions;

namespace Anfragen {

	public class Program {

		public static void Main(string[ ] args) {

			UserConsole terminal = new UserConsole( );
			IQuestionnaire questionnaire = new Questionnaire( terminal );
			questionnaire.Settings.WelcomeMessage = "Welcome to my questionnare";

			Func<Question, bool> validator = x=> x.Answer.Length > 0;
			var  errorMessage = "Plaese Provide a value";

			Question ask_name = new Prompt( "What's your name?" ).Validator( validator,errorMessage);
			Question ask_family = new Prompt( "What's your family?" ).Validator( validator,errorMessage); ;

			questionnaire
				.Add(ask_name)
				.Add(ask_family)
				;

			questionnaire.Start();

			bool add = true;

			// loop until there is a question to ask
			while (questionnaire.CanProceed) {

				if (add) {

					var confirm  = new Confirm("Are you older than 18?");

					confirm.Validator(x => {
						Confirm q = (Confirm)x;
						return q.PossibleAnswers.Contains(x.Answer);
					}, "Your value should be either 'Yes' or 'No'");

					questionnaire.Confirm(confirm);

					questionnaire.Prompt(new Prompt("How old are you?"));
					add = false;
				}

				questionnaire.Next();

			}

			questionnaire.End();

			// Print Processed questions
			foreach (Question q in questionnaire.ProcessedQuestions) {

				terminal.Printer.Write($"{q.Text} : {q.Answer}");
				terminal.Printer.WriteLine();

			}

			Console.ReadLine();
		}
	}
}
