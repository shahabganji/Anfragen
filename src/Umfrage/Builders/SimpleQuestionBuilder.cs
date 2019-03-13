using System;

using Umfrage.Abstractions;
using Umfrage.Builders.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;


namespace Umfrage.Builders
{
	class SimpleQuestionBuilder : ISimpleQuestionBuilder {

		private Func<IQuestion, IQuestion> _builderFunc;
		private readonly IQuestionBuilder _builder;

		internal SimpleQuestionBuilder(IQuestionBuilder builder) {
			_builder = builder;
		}

		public ISimpleQuestionBuilder Text(string text) {
			this._builderFunc = (q) => new Prompt(prompt: text) as IQuestion;
			return this;
		}

		public ISimpleQuestionBuilder WithHint(string hint) {

			this._builderFunc = this._builderFunc.Compose(question => {

				question.Hint = hint;

				return question;
			});

			return this;
		}

		public ISimpleQuestionBuilder WithDefaultAnswer(string defaultAnswer) {

			this._builderFunc = this._builderFunc.Compose(question => {

				question.DefaultAnswer= defaultAnswer;

				return question;
			});

			return this;
		}

		public ISimpleQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errorMessage = "") {
			this._builderFunc = this._builderFunc.Compose((question) => {

				question.Validator(validator, errorMessage);

				return question;
			});

			return this;
		}

		public ISimpleQuestionBuilder WithErrorMessage(string errorMessage) {

			this._builderFunc = this._builderFunc.Compose(question => {

				question.ErrorMessage = errorMessage;

				return question;
			});

			return this;
		}

		public IQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire) {

			this._builderFunc = this._builderFunc.Compose(question => {
				questionnaire.Add(question);
				return question;
			});

			this.Build();

			return _builder;
		}

		public IQuestion Build() {

			return this._builderFunc?.Invoke(null);

		}

		public ISimpleQuestionBuilder AsConfirm() {

			this._builderFunc = this._builderFunc.Compose(question => {
				Confirm q = new Confirm(question.Text , question.Hint , question.DefaultAnswer , questionnaire: question.Questionnaire);
				return q;
			});

			return this;
		}

	}
}
