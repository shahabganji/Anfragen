using System;
using System.Collections.Generic;
using System.Linq;
using Umfrage.Abstractions;
using Umfrage.Builders.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;

namespace Umfrage.Builders
{
	internal class ListQuestionBuilder : IListQuestionBuilder {

		private Func<IQuestion, IQuestion> _builderFunc;
		private readonly IQuestionBuilder _builder;

		internal ListQuestionBuilder(IQuestionBuilder builder)
		{
			_builder = builder;
		}


		public IListQuestionBuilder Text(string text) {
			this._builderFunc = (q) => new SelectableList(question: text);
			return this;
		}

		public IListQuestionBuilder WithHint(string hint) {

			this._builderFunc = this._builderFunc.Compose(question => {

				question.Hint = hint;

				return question;
			});

			return this;
		}

		public IListQuestionBuilder WithDefaultAnswer(string defaultAnswer) {

			this._builderFunc = this._builderFunc.Compose(question => {

				question.DefaultAnswer = defaultAnswer;

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errorMessage = "") {
			this._builderFunc = this._builderFunc.Compose((question) => {

				question.Validator(validator, errorMessage);

				return question;
			});

			return this;
		}

		public IListQuestionBuilder WithErrorMessage(string errorMessage) {

			this._builderFunc = this._builderFunc.Compose(question => {

				question.ErrorMessage = errorMessage;

				return question;
			});

			return this;
		}

		//public IListQuestionBuilder AddOption(IOption option) {

		//	this._builderFunc = this._builderFunc.Compose((question) => {

		//		(question as SelectableList).AddOption(option);

		//		return question;
		//	});

		//	return this;
		//}
		public IListQuestionBuilder AddOptions(IEnumerable<IOption> options) {

			this._builderFunc = this._builderFunc.Compose((question) => {

				foreach (IOption option in options) {
					(question as SelectableList)?.AddOption(option);
				}

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AddOptions(IEnumerable<string> options) {

			this._builderFunc = this._builderFunc.Compose((question) =>
			{
				var opts = options.Select( o=> new QuestionOption(o) );

				this.AddOptions(opts);

				return question;
			});

			return this;
		}

		public IListQuestionBuilder AsRadioList() {

			this._builderFunc = this._builderFunc.Compose(question => {
				(question as SelectableList).ShowAsRadio = true;
				return question;
			});

			return this;
		}

		public IListQuestionBuilder AsCheckList() {

			this._builderFunc = this._builderFunc.Compose(question => {
				SelectableList q = (question as SelectableList );
				CheckList q2 = new CheckList(q.Text, q.Options, q.Hint , q.DefaultAnswer, q.VisibleOptions, q.Questionnaire);
				return q2;
			});

			return this;
		}

		public IQuestion Build() {

			return this._builderFunc?.Invoke(null);

		}

		public IListQuestionBuilder WithVisibleOptions(int visibleItems) {

			this._builderFunc = this._builderFunc.Compose(question => {

				(question as SelectableList).VisibleOptions = visibleItems;

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

	}
}
