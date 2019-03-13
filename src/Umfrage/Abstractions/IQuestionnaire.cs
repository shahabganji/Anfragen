using System.Collections.Generic;

using Umfrage.Builders.Abstractions;

namespace Umfrage.Abstractions
{
	public interface IQuestionnaire
	{

		bool CanProceed { get; }

		//IQuestionBuilder Builder { get; }

		QuestionnaireSetting Settings { get; set; }

		IUserTerminal Terminal { get; }

		IEnumerable<IBranch> Branches { get; }
		IEnumerable<IQuestion> Questions { get; } // questions in main branch 

		IEnumerable<IQuestion> ProcessedQuestions { get; }

		IQuestion PreviousQuestion { get; }
		IQuestion CurrentQuestion { get; }
		IQuestion NextQuestion { get; }

		IQuestionnaire Start(); // prints the first question
		void End(); // prints the first question

		IQuestionnaire Next(); // forwards to the next step in the current branch
		IQuestionnaire Prev(); // backwards to the previous step in the current branch, ? what would happen if the previous step is not from the current branch

		IQuestionnaire GoToBranch(string branchName); // oes to first step of the specified branch
		IQuestionnaire GotToStep(int step, string branchName = null); // goes to the specified step of the specified branch


		IQuestionnaire Add(IBranch branch); // adds questions to the main branch unless a branch is provided
		IQuestionnaire Add(IQuestion question, IBranch branch = null, bool here = false); // adds questions to the main branch unless a branch is provided

	}
}
