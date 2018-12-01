using Anfragen.Abstractions;
using System;
using System.Collections.Generic;

namespace Anfragen {

	public interface ISelectableListQuestionBuilder {
		ISelectableListQuestionBuilder New(string text);
		ISelectableListQuestionBuilder AddValidation(Func<IQuestion, bool> validator, string errormessage = "");
		ISelectableListQuestionBuilder WithErrorMessage(string errorMessage);
		ISelectableListQuestionBuilder WithHint(string hint);
		ISelectableListQuestionBuilder WithVisibleOptions(int visibleItems);
		ISelectableListQuestionBuilder AddOptions(IEnumerable<IOption> options);
		ISelectableListQuestionBuilder AddOption(IOption option);
		ISelectableListQuestionBuilder AsRadioList();
		ISelectableListQuestionBuilder AsCheckList();
		ISelectableListQuestionBuilder AddToQuestionnaire(IQuestionnaire questionnaire);
		IQuestion Build();
	}
}
