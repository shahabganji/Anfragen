using Anfragen.Abstractions;
using Anfragen.Builders.Abstractions;
using Anfragen.Extensions;
using Anfragen.Implementations;

using System;

namespace Anfragen.Builders {
	class SimpleQuestionBuilder : ISimpleQuestionBuilder {

		private Func<IQuestion, IQuestion> builderFunc;

		public ISimpleQuestionBuilder New(string text) {
			this.builderFunc = (q) => new Prompt(prompt: text) as IQuestion;
			return this;
		}

		public ISimpleQuestionBuilder WithHint(string hint) {

			this.builderFunc = this.builderFunc.Compose(question => {

				question.Hint = hint;

				return question;
			});

			return this;
		}

		public ISimpleQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errormessage = "") {
			this.builderFunc = this.builderFunc.Compose((question) => {

				question.Validator(validator, errormessage);

				return question;
			});

			return this;
		}

		public ISimpleQuestionBuilder WithErrorMessage(string errorMessage) {

			this.builderFunc = this.builderFunc.Compose(question => {

				question.ErrorMessage = errorMessage;

				return question;
			});

			return this;
		}

		public ISimpleQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire) {

			this.builderFunc = this.builderFunc.Compose(question => {
				questionnaire.Add(question);
				return question;
			});

			return this;
		}

		public IQuestion Build() {

			return this.builderFunc?.Invoke(null);

		}

		public ISimpleQuestionBuilder AsConfirm() {

			this.builderFunc = this.builderFunc.Compose(question => {
				Confirm q = new Confirm(question.Text , question.Hint , questionnaire: question.Questionnaire);
				return q;
			});

			return this;
		}

	}
}
