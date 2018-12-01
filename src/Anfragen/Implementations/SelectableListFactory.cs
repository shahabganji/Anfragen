using Anfragen.Extensions;
using Anfragen.Abstractions;
using System;
using System.Collections.Generic;

namespace Anfragen.Implementations {
	public class SelectableListQuestionBuilder : ISelectableListQuestionBuilder {

		private Func<IQuestion, IQuestion> builderFunc;

		public ISelectableListQuestionBuilder New(string text) {
			this.builderFunc = (q) => new SelectableList(question: text);
			return this;
		}


		public ISelectableListQuestionBuilder WithHint(string hint) {

			this.builderFunc = this.builderFunc.Compose(question => {

				question.Hint = hint;

				return question;
			});

			return this;
		}

		public ISelectableListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errormessage = "") {
			this.builderFunc = this.builderFunc.Compose((question) => {

				question.Validator(validator, errormessage);

				return question;
			});

			return this;
		}

		public ISelectableListQuestionBuilder WithErrorMessage(string errorMessage) {

			this.builderFunc = this.builderFunc.Compose(question => {

				question.ErrorMessage = errorMessage;

				return question;
			});

			return this;
		}

		public ISelectableListQuestionBuilder AddOption(IOption option) {

			this.builderFunc = this.builderFunc.Compose((question) => {

				(question as SelectableList).AddOption(option);

				return question;
			});

			return this;
		}
		public ISelectableListQuestionBuilder AddOptions(IEnumerable<IOption> options) {

			this.builderFunc = this.builderFunc.Compose((question) => {

				foreach (IOption option in options) {
					(question as SelectableList).AddOption(option);
				}

				return question;
			});

			return this;
		}

		public ISelectableListQuestionBuilder AsRadioList() {

			this.builderFunc = this.builderFunc.Compose(question => {
				(question as SelectableList).ShowAsRadio = true;
				return question;
			});

			return this;
		}

		public ISelectableListQuestionBuilder AsCheckList() {

			this.builderFunc = this.builderFunc.Compose(question => {
				SelectableList q = (question as SelectableList );
				var q2 = new CheckList(q.Text, q.Options, q.VisibleOptions, q.Questionnaire);
				return q2;
			});

			return this;
		}

		public IQuestion Build() {

			return this.builderFunc?.Invoke(null);

		}

		public ISelectableListQuestionBuilder WithVisibleOptions(int visibleItems) {

			this.builderFunc = this.builderFunc.Compose(question => {

				(question as SelectableList).VisibleOptions = visibleItems;

				return question;
			});

			return this;
		}

		public ISelectableListQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire) {

			this.builderFunc = this.builderFunc.Compose(question => {
				questionnaire.Add(question);
				return question;
			});

			return this;
		}
	}
}
