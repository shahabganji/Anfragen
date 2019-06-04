using System;

using Umfrage.Abstractions;

namespace Umfrage.Builders.Abstractions
{

	public interface ISimpleQuestionBuilder {
		ISimpleQuestionBuilder Text(string text);

		ISimpleQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errorMessage = "");
		ISimpleQuestionBuilder WithErrorMessage(string errorMessage);
		ISimpleQuestionBuilder WithHint(string hint);
		ISimpleQuestionBuilder WithDefaultAnswer(string defaultAnswer);
		IQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire);

		ISimpleQuestionBuilder AsConfirm();

		IQuestion Build();
	}
}
