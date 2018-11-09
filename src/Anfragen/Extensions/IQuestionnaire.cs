using Anfragen.Interfaces;

namespace Anfragen.Extensions {

    public static class IQuestionnaireExtensions {

        public static IQuestionnaire Prompt( this IQuestionnaire questionnaire, IPrompt prompt ) {
            return questionnaire.Add( prompt );
        }

        public static IQuestionnaire Confirm( this IQuestionnaire questionnaire, IConfirm confirm ) {
            return questionnaire.Add( confirm );
        }
    }
}
