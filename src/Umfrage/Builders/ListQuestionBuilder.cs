using Umfrage.Abstractions;
using Umfrage.Builders.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;

using System;
using System.Collections.Generic;

namespace Umfrage.Builders {
	class ListQuestionBuilder : IListQuestionBuilder {

		private Func<IQuestion, IQuestion> builderFunc;

		public IListQuestionBuilder New(string text) {
			this.builderFunc = (q) => new SelectableList(question: text);
			return this;
		}

		public IListQuestionBuilder WithHint(string hint) {

			this.builderFunc = this.builderFunc.Compose(question => {

				question.Hint = hint;

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errormessage = "") {
			this.builderFunc = this.builderFunc.Compose((question) => {

				question.Validator(validator, errormessage);

				return question;
			});

			return this;
		}

		public IListQuestionBuilder WithErrorMessage(string errorMessage) {

			this.builderFunc = this.builderFunc.Compose(question => {

				question.ErrorMessage = errorMessage;

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AddOption(IOption option) {

			this.builderFunc = this.builderFunc.Compose((question) => {

				(question as SelectableList).AddOption(option);

				return question;
			});

			return this;
		}
		public IListQuestionBuilder AddOptions(IEnumerable<IOption> options) {

			this.builderFunc = this.builderFunc.Compose((question) => {

				foreach (IOption option in options) {
					(question as SelectableList).AddOption(option);
				}

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AsRadioList() {

			this.builderFunc = this.builderFunc.Compose(question => {
				(question as SelectableList).ShowAsRadio = true;
				return question;
			});

			return this;
		}

		public IListQuestionBuilder AsCheckList() {

			this.builderFunc = this.builderFunc.Compose(question => {
				SelectableList q = (question as SelectableList );
				CheckList q2 = new CheckList(q.Text, q.Options, q.VisibleOptions, q.Questionnaire);
				return q2;
			});

			return this;
		}

		public IQuestion Build() {

			return this.builderFunc?.Invoke(null);

		}

		public IListQuestionBuilder WithVisibleOptions(int visibleItems) {

			this.builderFunc = this.builderFunc.Compose(question => {

				(question as SelectableList).VisibleOptions = visibleItems;

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire) {

			this.builderFunc = this.builderFunc.Compose(question => {
				questionnaire.Add(question);
				return question;
			});

			return this;
		}

	}
}
