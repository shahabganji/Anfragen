using Anfragen.Extensions;
using Anfragen.Interfaces;
using System;
using System.Collections.Generic;

namespace Anfragen.Implementations {
	public class RawList : Question {

		private IList<IOption> _options;
		public IEnumerable<IOption> Options => this._options;

		public int VisibleOptions { get; protected set; }

		public RawList(string question, IEnumerable<IOption> options = null, int visibleOptions = 3, IQuestionnaire questionnaire = null) : base(question, questionnaire) {
			this._options = new List<IOption>();
			this.VisibleOptions = visibleOptions;
		}

		protected override Question TakeAnswer() {
			IUserTerminal terminal = this.Terminal;

			int column = Console.CursorLeft;
			int line = Console.CursorTop;

			int activeOption = -1;
			drawOptions(terminal);

			Console.SetCursorPosition(column, line);

			bool answered = false;

			while (!answered) {

				ConsoleKeyInfo keyInfo = Console.ReadKey();
				Console.SetCursorPosition(column, line);

				switch (keyInfo.Key) {
					case ConsoleKey.Enter:

						this.Answer = activeOption > -1 ? this._options[ activeOption ].Text : null;
						answered = true;
						break;
					case ConsoleKey.UpArrow:

						this.ClearLines(line + 1, line + this._options.Count);
						Console.SetCursorPosition(column, line);

						--activeOption;
						if (activeOption == -1) {
							activeOption = this._options.Count - 1;
						}


						this.drawOptions(terminal, activeOption);
						Console.SetCursorPosition(column, line);

						this.ClearAnswer(line);
						terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
						terminal.Printer.Write(this._options[activeOption].Text);

						break;
					case ConsoleKey.DownArrow:

						this.ClearLines(line + 1, line + this._options.Count);
						Console.SetCursorPosition(column, line);

						++activeOption;
						if (activeOption == this._options.Count) {
							activeOption = 0;
						}

						this.drawOptions(terminal, activeOption);
						Console.SetCursorPosition(column, line);

						this.ClearAnswer(line);
						terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
						terminal.Printer.Write(this._options[ activeOption ].Text);
						break;
					default:
						this.ClearAnswer(line);
						break;
				}
			}

			this.ClearLines(line + 1, line + this._options.Count);
			this.State = this.Validate() ? QuestionStates.Valid : QuestionStates.Invalid;

			// resets the cursor to the nxt line, 
			// because of the options the cursor might be in wrong position for the next question
			Console.SetCursorPosition(0, line + 1); 

			terminal.ResetColor();
			return this;

		}

		private void drawOptions(IUserTerminal terminal, int active = 0) {
			// set terminal to next line
			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
			terminal.Printer.WriteLine();

			for (int index = 0; index < this.VisibleOptions; index++) {

				if (index == active) {
					terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
				}
				terminal.Printer.Write("  > ");

				terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
				terminal.Printer.WriteLine($"{this._options[ index ].Text}");

			}
		}

		public RawList AddOption(IOption option) {
			this._options.Add(option);
			return this;
		}

	}

	public class ListOption : IOption {
		public string Text { get; }

		public ListOption(string option) {
			this.Text = option;
		}
	}

	public interface IOption {
		string Text { get; }
	}
}
