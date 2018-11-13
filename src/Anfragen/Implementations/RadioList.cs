using System;
using System.Collections.Generic;
using System.Text;
using Anfragen.Interfaces;

namespace Anfragen.Implementations {
	public class RadioList : RawList {
		public RadioList(string question, IEnumerable<IOption> options = null, int visibleOptions = 3, IQuestionnaire questionnaire = null) : 
			base(question, options, visibleOptions, questionnaire) {
		}

		protected override void PrintIndividualOption(int active, IOption option, bool isActive) {

			var terminal = this.Terminal;

			// which option is selected
			if (isActive) {
				terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
				terminal.Printer.Write("  (o) ");
			} else {
				terminal.Printer.Write("  ( ) ");
			}
			//	**********************************************************

			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
			terminal.Printer.WriteLine($"{option.Text}");

		}

	}
}
