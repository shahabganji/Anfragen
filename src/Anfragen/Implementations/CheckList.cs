using Anfragen.Extensions;
using Anfragen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Anfragen.Implementations {
	public class CheckList : SingleSelect {
		public CheckList(string question, IEnumerable<IOption> options = null, int visibleOptions = 3, IQuestionnaire questionnaire = null) : base(question, options, visibleOptions, questionnaire) {
		}

		protected override Question TakeAnswer() {

			IUserTerminal terminal = this.Terminal;

			int column = Console.CursorLeft;
			int line = Console.CursorTop;

			int activeOption = -1;
			if (this.State == QuestionStates.Initilaized) {
				this.DrawOptions();
			}

			Console.SetCursorPosition(column, line);
			activeOption = this.HandleInput(column, line, activeOption);

			this.State = this.Validate() ? QuestionStates.Valid : QuestionStates.Invalid;

			if (this.State == QuestionStates.Invalid) {
				Console.SetCursorPosition(0, line + this.VisibleOptions + 1);
				this.PrintValidationErrors();
				Console.SetCursorPosition(column, line);
				this.TakeAnswer();

			} else {
				this.ClearLines(line + 1, line + this.VisibleOptions + 1);
			}

			// resets the cursor to the next line, 
			// because of the options the cursor might be in wrong position for the next question
			Console.SetCursorPosition(0, line + 1);

			terminal.ResetColor();
			return this;

		}

		protected virtual int HandleInput(int column, int line, int activeOptionIndex) {

			IUserTerminal terminal = this.Terminal;

			bool answered = false;

			while (!answered) {

				ConsoleKeyInfo keyInfo = Console.ReadKey();

				switch (keyInfo.Key) {
					case ConsoleKey.Enter:
						this.Answer = this._options
										.Where(op => op.Selected == true)
										.Select( opt => opt.Text )
										.Aggregate((a, b) => $"{a}, {b}");

						answered = true;
						break;
					case ConsoleKey.UpArrow:
						Console.SetCursorPosition(column, line);

						this.ClearLines(line + 1, line + this._options.Count);
						Console.SetCursorPosition(column, line);

						--activeOptionIndex;
						if (activeOptionIndex == -1) {
							activeOptionIndex = this._options.Count - 1;
						}

						this.DrawOptions(activeOptionIndex);
						Console.SetCursorPosition(column, line);

						break;
					case ConsoleKey.DownArrow:
						Console.SetCursorPosition(column, line);

						this.ClearLines(line + 1, line + this._options.Count);
						Console.SetCursorPosition(column, line);

						++activeOptionIndex;
						if (activeOptionIndex == this._options.Count) {
							activeOptionIndex = 0;
						}

						this.DrawOptions(activeOptionIndex);
						Console.SetCursorPosition(column, line);

						break;
					case ConsoleKey.Spacebar:

						Console.SetCursorPosition(column, line);

						IOption selectedItem = activeOptionIndex > -1 ? this._options[ activeOptionIndex ] : null;
						if (this.Type == ListType.Check && selectedItem != null) {
							selectedItem.Selected = !selectedItem.Selected;
							this.DrawOptions(activeOptionIndex);
						}

						this.ClearAnswer(line);
						terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
						var selected = this._options
										.Where(op => op.Selected == true)
										.Select( opt => opt.Text )
										.Aggregate((a, b) => $"{a}, {b}");
						terminal.Printer.Write(selected);

						break;

					case ConsoleKey.F2:
					default:
						//this.ClearLines(line + 1, line + this._options.Count);
						//Console.SetCursorPosition(column, line);
						//this.DrawOptions(activeOptionIndex);
						//Console.SetCursorPosition(column, line);
						break;
				}
			}

			return activeOptionIndex;
		}

		protected override void DrawOptions(int active = -1) {
			IUserTerminal terminal = this.Terminal;

			// set terminal to next line
			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
			terminal.Printer.WriteLine();

			int page = active / this.VisibleOptions;
			List<IOption> visible_items = this.Options.ToList().Skip(page * this.VisibleOptions).Take(this.VisibleOptions).ToList();

			for (int index = 0; index < visible_items.Count; index++) {
				this.PrintIndividualOption(visible_items[ index ], visible_items[ index ].Selected , active % this.VisibleOptions == index);
			}
		}

		protected override void PrintIndividualOption(IOption option, bool isActive, bool highlight = false) {
			IUserTerminal terminal = this.Terminal;

			if (highlight ) {
				terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
			}

			// which option is selected
			if (isActive) {
				terminal.Printer.Write("    [x] ");
			} else {
				terminal.Printer.Write("    [ ] ");
			}
			//	**********************************************************
			terminal.Printer.WriteLine($"{option.Text}");
			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
		}

	}
}
