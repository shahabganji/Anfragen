using Umfrage.Abstractions;
using Umfrage.Implementations;

namespace Umfrage.Extensions {

	public static class IQuestionnaireExtensions {

		public static IQuestionnaire Prompt(this IQuestionnaire questionnaire, Prompt prompt) {

			if (prompt == null) {
				throw new System.ArgumentNullException(nameof(prompt));
			}

			return questionnaire.Add(prompt);
		}

		public static IQuestionnaire Confirm(this IQuestionnaire questionnaire, Confirm confirm) {

			if (confirm == null) {
				throw new System.ArgumentNullException(nameof(confirm));
			}

			return questionnaire.Add(confirm);
		}

		public static IQuestionnaire SingleSelectListQuestion(this IQuestionnaire questionnaire, SelectableList question) {

			if (question == null) {
				throw new System.ArgumentNullException(nameof(question));
			}

			return questionnaire.Add(question);
		}

		public static IQuestionnaire MultiSelectListQuestion(this IQuestionnaire questionnaire, CheckList question) {

			if (question == null) {
				throw new System.ArgumentNullException(nameof(question));
			}

			return questionnaire.Add(question);
		}
	}
}
