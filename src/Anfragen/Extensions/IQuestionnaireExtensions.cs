using Anfragen.Abstractions;
using Anfragen.Implementations;

namespace Anfragen.Extensions {

	public static class IQuestionnaireExtensions {

        public static IQuestionnaire Prompt( this IQuestionnaire questionnaire, Prompt prompt ) {

			if (prompt == null) {
				throw new System.ArgumentNullException(nameof(prompt));
			}

			return questionnaire.Add( prompt );
        }

        public static IQuestionnaire Confirm( this IQuestionnaire questionnaire, Confirm confirm ) {

			if (confirm == null) {
				throw new System.ArgumentNullException(nameof(confirm));
			}

			return questionnaire.Add( confirm );
        }
    }
}
