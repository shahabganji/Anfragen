﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Anfragen.Interfaces
{
	public interface IQuestionnaire
	{

		bool CanProceed { get; }

		QuestionnaireSetting Settings { get; set; }

		IUserTerminal Terminal { get; }

		IEnumerable<IBranch> Branches { get; }
		IEnumerable<Question> Questions { get; } // questions in main branch 

		IEnumerable<Question> ProcessedQuestions { get; }

		Question PreviousQuestion { get; }
		Question CurrentQuestion { get; }
		Question NextQuestion { get; }

		IQuestionnaire Start(); // prints the first question
		void End(); // prints the first question

		IQuestionnaire Next(); // forwards to the next step in the current branch
		IQuestionnaire Prev(); // backwards to the previous step in the current branch, ? what would happen if the previous step is not from the current branch

		IQuestionnaire GoToBranch(string branchName); // oes to first step of the specified branch
		IQuestionnaire GotToStep(int step, string branchName = null); // goes to the specified step of the specified branch


		IQuestionnaire Add(IBranch branch); // adds questions to the main branch unless a branch is provided
		IQuestionnaire Add(Question question, IBranch branch = null, bool here = false); // adds questions to the main branch unless a branch is provided

	}
}
