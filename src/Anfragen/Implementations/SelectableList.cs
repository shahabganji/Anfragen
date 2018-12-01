using Anfragen.Extensions;
using Anfragen.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Anfragen.Implementations {
	
	class SelectableList : Question {

		protected IList<IOption> _options;
		public IEnumerable<IOption> Options => this._options;

		internal bool ShowAsRadio { get; set; } = false;

		public int VisibleOptions { get; internal set; }
		public SelectableList AddOption(IOption option) {
			this._options.Add(option);
			return this;
		}

		internal SelectableList(string question, IEnumerable<IOption> options = null, int visibleOptions = 4, IQuestionnaire questionnaire = null) : 
			base(question, questionnaire) {

			this._options = new List<IOption>();

			if( options != null) {
				foreach( var option in options) {
					this._options.Add(option);
				}
			}

			this.VisibleOptions = visibleOptions;
		}

		protected override IQuestion TakeAnswer() {

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
						this.Answer = activeOptionIndex > -1 ? this._options[ activeOptionIndex ].Text : null;
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

						this.ClearAnswer(line);
						terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
						terminal.Printer.Write(this._options[ activeOptionIndex ].Text);

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

						this.ClearAnswer(line);
						terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
						terminal.Printer.Write(this._options[ activeOptionIndex ].Text);
						break;
					default:
						break;
				}
			}

			return activeOptionIndex;
		}

		protected virtual void DrawOptions(int active = -1) {

			IUserTerminal terminal = this.Terminal;

			// set terminal to next line
			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
			terminal.Printer.WriteLine();

			int page = active / this.VisibleOptions;
			List<IOption> visible_items = this._options.Skip(page * this.VisibleOptions).Take(this.VisibleOptions).ToList();

			for (int index = 0; index < visible_items.Count; index++) {
				
					this.PrintIndividualOption(visible_items[ index ], index == (active >= this.VisibleOptions ? active - this.VisibleOptions : active));
				
			}
		}

		protected virtual void PrintIndividualOption(IOption option, bool isActive, bool highlight = false) {

			IUserTerminal terminal = this.Terminal;

			// which option is selected
			if (isActive) {

				terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;

				if (this.ShowAsRadio) {
					terminal.Printer.Write("    (O) ");
				} else  {
					terminal.Printer.Write("    > ");
				}

			} else {

				if (this.ShowAsRadio) {
					terminal.Printer.Write("    ( ) ");
				} else {
					terminal.Printer.Write("    > ");
				}

			}
			//	**********************************************************

			terminal.Printer.WriteLine($"{option.Text}");
			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
		}

	
	}
}
