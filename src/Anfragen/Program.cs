using Anfragen.Extensions;
using Anfragen.Implementations;
using Anfragen.Interfaces;
using System;
using System.Collections.Generic;
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
			Question ask_family = new Prompt( "What's your family?" ).Validator( validator,errorMessage);

			
			var preferable_language = new RawList( "What's your favorite language?");
			preferable_language.Validator(x=> x.Answer != null , "You must select an option");

			preferable_language
				.AddOption(new ListOption("Persian"))
				.AddOption(new ListOption("English"))
				.AddOption(new ListOption("Italian"))
				.AddOption(new ListOption("Spanish"))
				.AddOption(new ListOption("French"))
				.AddOption(new ListOption("German"));


			questionnaire
				.Add(preferable_language)
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

					var age_prompt = new Prompt("How old are you?");
					age_prompt.Validator(x => int.Parse(x.Answer) >= 18 , "Your must be older than 18");
					questionnaire.Prompt(age_prompt);
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
