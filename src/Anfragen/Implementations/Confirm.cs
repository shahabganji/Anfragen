using Anfragen.Interfaces;
using Anfragen.Extensions;

using System;
using System.Collections.Generic;
using System.IO;

namespace Anfragen.Implementations {

	public class Confirm : Question {

        public IList<string> PossibleAnswers { get; }

        public Confirm( string question, string[ ] possibleAnswers = null ) {
            this.Text = question;
			this.Hint = "Yes/No";

			this.PossibleAnswers = possibleAnswers ?? new[ ] { "Yes", "No" };
        }
			   		
		protected override Question TakeAnswer() {

			int cursorTop = Console.CursorTop,
				cursorLeft= Console.CursorLeft;

			IUserTerminal terminal = this.Terminal;
			
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