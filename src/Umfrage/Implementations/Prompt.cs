using System;
using Umfrage.Abstractions;
using Umfrage.Extensions;

namespace Umfrage.Implementations {

	public class Prompt : Question {

		public Prompt(string prompt, string hint = "", string defaultAnswer = null, IQuestionnaire questionnaire = null) :
			base(prompt, questionnaire) {

			this.Hint = hint;
		}

		protected override IQuestion TakeAnswer() {

			IUserTerminal terminal = this.Terminal;

			// this should be before ReadLine, 
			// because ReadLine will eliminate the current Lef position and reset it to 0
			int cursorLeft = Console.CursorLeft;

			// always set the color of terminal to AnswerColor
			terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
			this.Answer = terminal.Scanner.ReadLine();

			// this should be after ReadLine because 
			// before the user enters the input he/she might resize the console, 
			// hence the top will change 
			int cursorTop = Console.CursorTop;

			if (this.Answer.Trim().Length == 0 && this.DefaultAnswer != null) {
				Console.SetCursorPosition(left: cursorLeft, top: cursorTop - 1);
				terminal.Printer.Write(this.DefaultAnswer);
				this.Answer = this.DefaultAnswer;
			}

			bool result = this.Validate( );
			this.State = result ? QuestionStates.Valid : QuestionStates.Invalid;

			if (result) {
				this.ClearLine(cursorTop);
			} else {

				this.PrintValidationErrors();

				// -1 because of read line
				int line = cursorTop - 1;
				this.ClearAnswer(line: line);
				Console.SetCursorPosition(left: cursorLeft, top: line);
				return this.TakeAnswer();
			}

			terminal.ResetColor();

			return this;
		}

	}
}