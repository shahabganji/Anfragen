using Anfragen.Abstractions;

using System;
using System.Collections.Generic;

namespace Anfragen.Builders.Abstractions {

	public interface IListQuestionBuilder {

		IListQuestionBuilder New(string text);

		IListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errormessage = "");
		IListQuestionBuilder WithErrorMessage(string errorMessage);
		IListQuestionBuilder WithHint(string hint);
		IListQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire);


		IListQuestionBuilder WithVisibleOptions(int visibleItems);
		IListQuestionBuilder AddOptions(IEnumerable<IOption> options);
		IListQuestionBuilder AddOption(IOption option);
		IListQuestionBuilder AsRadioList();
		IListQuestionBuilder AsCheckList();

		IQuestion Build();
	}

	public interface ISimpleQuestionBuilder {

		ISimpleQuestionBuilder New(string text);

		ISimpleQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errormessage = "");
		ISimpleQuestionBuilder WithErrorMessage(string errorMessage);
		ISimpleQuestionBuilder WithHint(string hint);
		ISimpleQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire);

		ISimpleQuestionBuilder AsConfirm();

		IQuestion Build();

	}

	public interface IQuestionBuilder {
		ISimpleQuestionBuilder Simple();
		IListQuestionBuilder List();
	}

}
