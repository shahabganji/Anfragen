using System;
using System.Collections.Generic;
using System.Text;
using Anfragen.Extensions;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {
	public class CheckList : RawList {
		public CheckList(string question, IEnumerable<IOption> options = null, int visibleOptions = 3, IQuestionnaire questionnaire = null) : base(question, options, visibleOptions, questionnaire) {

			this.OtherKyesPressed += this.CheckList_OtherKyesPressed;

		}

		private bool CheckList_OtherKyesPressed(OtherKeysPressedEventArgs e) {

			if( e.KeyInfo.Key == ConsoleKey.Spacebar) {

				int optionLine = ( e.ActiveIndex % this.VisibleOptions ) + 1 + Console.CursorTop;
				this.ClearLine(optionLine);
				this.PrintIndividualOption(e.ActiveIndex, e.Option, true);

			}

			return false;
		}

		protected override void PrintIndividualOption(int active, IOption option, bool isActive) {
			var terminal = this.Terminal;

			// which option is selected
			if (isActive) {
				terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
				terminal.Printer.Write("  [x] ");
			} else {
				terminal.Printer.Write("  [ ] ");
			}
			//	**********************************************************

			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
			terminal.Printer.WriteLine($"{option.Text}");
		}


	}
}
