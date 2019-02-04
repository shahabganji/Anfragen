using Umfrage.Abstractions;

using System;
using System.Collections.Generic;

namespace Umfrage.Builders.Abstractions {

	public interface IListQuestionBuilder {

		IListQuestionBuilder Text(string text);

		IListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errorMessage = "");
		IListQuestionBuilder WithErrorMessage(string errorMessage);
		IListQuestionBuilder WithHint(string hint);
		IListQuestionBuilder WithDefaultAnswer(string defaultAnswer);
		IQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire);
		
		IListQuestionBuilder WithVisibleOptions(int visibleItems);
		IListQuestionBuilder AddOptions(IEnumerable<IOption> options);
		IListQuestionBuilder AddOption(IOption option);
		IListQuestionBuilder AsRadioList();
		IListQuestionBuilder AsCheckList();

		IQuestion Build();
	}

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

	public interface IQuestionBuilder {
		ISimpleQuestionBuilder Simple();
		IListQuestionBuilder List();
	}

}
