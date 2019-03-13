using System;
using System.Collections.Generic;

using Umfrage.Abstractions;

namespace Umfrage.Builders.Abstractions
{
	public interface IListQuestionBuilder {

		IListQuestionBuilder Text(string text);

		IListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errorMessage = "");
		IListQuestionBuilder WithErrorMessage(string errorMessage);
		IListQuestionBuilder WithHint(string hint);
		IListQuestionBuilder WithDefaultAnswer(string defaultAnswer);
		IQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire);

		IListQuestionBuilder WithVisibleOptions(int visibleItems);
		IListQuestionBuilder AddOptions(IEnumerable<IOption> options);
		IListQuestionBuilder AddOptions(IEnumerable<string> options);
		//IListQuestionBuilder AddOption(IOption option);
		IListQuestionBuilder AsRadioList();
		IListQuestionBuilder AsCheckList();

		IQuestion Build();
	}
}
