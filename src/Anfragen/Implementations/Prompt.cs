using Anfragen.Extensions;
using Anfragen.Interfaces;
using System;

namespace Anfragen.Implementations {

	public class Prompt : Question {

		public Prompt(string prompt, string hint = "", IQuestionnaire questionnaire = null) : base(questionnaire) {
			this.Text = prompt;
			this.Hint = hint;
		}

		protected override Question TakeAnswer() {

			int cursorTop = Console.CursorTop,
				cursorLeft= Console.CursorLeft;

			IUserTerminal terminal = this.Terminal;

			// always set the color of terminal to AnswerColor
			terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;

			this.Answer = terminal.Scanner.ReadLine();

			bool result = this.Validate();
			this.State = result ? QuestionStates.Valid : QuestionStates.Invalid;

			if (result) {
				this.ClearLine(cursorTop + 1);
			} else {

				terminal.ForegroundColor = this.Questionnaire.Settings.ValidationIconColor;
				terminal.Printer.Write(this.Questionnaire.Settings.ValidationIcon + " ");

				terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
				this.PrintValidationErrors();

				Console.SetCursorPosition(left: cursorLeft, top: cursorTop);
				return this.TakeAnswer();
			}

			terminal.ResetColor();

			return this;
		}

	}
}