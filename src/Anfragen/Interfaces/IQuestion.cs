using System;
using System.Collections.Generic;
using System.Linq;

namespace Anfragen.Interfaces {

    public enum QuestionStates {
        Initilaized, Asked, Answered, Valid, NotValid, Finished
    }

    public interface IQuestion {
        QuestionStates State { get; }
        string QuestionIcon { get; }
        string Question { get; }
        string Answer { get; }
        IQuestion Ask( IPrinter printer ); // prints the question
        IQuestion TakeAnswer( ); // waits for the user to answer the question
        IQuestion Validate( Func<IQuestion, bool> validator = null ); // if any validator provided calls the custom validation, otherwise calls the default implementation
        IQuestion PrintValidationErrors( ); // Prints the validation errors
        void Finish( ); // the finishing touches of the question, for example rendering some extra texts
    }


    public interface IBranch {
        string Name { get; } // the name of the branch
        IEnumerable<IQuestion> Questions { get; }
        IBranch Add( IQuestion question, int? position = null ); // adds a question ton the end of the list 
    }

    public interface IQuestionnaire {

        IEnumerable<IBranch> Branches { get; }
        IEnumerable<IQuestion> Questions { get; } // questions in main branch 

        IQuestion PreviousQuestion { get; }
        IQuestion CurrentQuestion { get; }
        IQuestion NextQuestion { get; }

        IQuestionnaire Start( ); // prints the first question
        void End( ); // prints the first question

        IQuestionnaire GoToNextStep( ); // forwards to the next step in the current branch
        IQuestionnaire GoToPreviousStep( ); // backwards to the previous step in the current branch, ? what would happen if the previous step is not from the current branch

        IQuestionnaire GoToBranch( string branchName ); // oes to first step of the specified branch
        IQuestionnaire GotToStep( int step, string branchName = null ); // goes to the specified step of the specified branch


        IQuestionnaire Add( IBranch branch ); // adds questions to the main branch unless a branch is provided
        IQuestionnaire Add( IQuestion question, IBranch branch = null, bool here = false ); // adds questions to the main branch unless a branch is provided

    }
}