using System;
using System.Collections.Generic;
using System.Linq;

namespace Anfragen.Interfaces {

    public enum QuestionStates {
        Initilaized, Asked, Answered, Valid, NotValid, Finished
    }

    public interface IQuestion {

        string Hint { get; }
        string Answer { get; }
        string Question { get; }

        QuestionStates State { get; }

        IQuestion Ask( IPrinter printer ); // prints the question
        IQuestion TakeAnswer( ); // waits for the user to answer the question
        IQuestion Validate( Func<IQuestion, bool> validator = null ); // if any validator provided calls the custom validation, otherwise calls the default implementation
        IQuestion PrintValidationErrors( ); // Prints the validation errors
        void Finish( Action done = null ); // the finishing touches of the question, for example rendering some extra texts
        void PrintResult( IPrinter printer );
    }


    public interface IBranch {
        string Name { get; } // the name of the branch
        IEnumerable<IQuestion> Questions { get; }
        IBranch Add( IQuestion question, int? position = null ); // adds a question ton the end of the list 
    }

    public interface IQuestionnaire {

        bool CanProceed { get; }

        QuestionnaireSetting Settings { get; set; }

        IEnumerable<IBranch> Branches { get; }
        IEnumerable<IQuestion> Questions { get; } // questions in main branch 

        IEnumerable<IQuestion> ProcessedQuestions { get; }

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


        IQuestionnaire Validate( Func<IQuestion, bool> validator = null );

    }

    public class QuestionnaireSetting {

        public string QuestionIcon { get; set; } = "?";
        public ConsoleColor HintColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor AnswerColor { get; set; } = ConsoleColor.DarkCyan;
        public ConsoleColor QuestionColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor QuestionIconColor { get; set; } = ConsoleColor.DarkGreen;

        public string ValidationIcon { get; set; } = ">>";
        public ConsoleColor ValidationIconColor { get; set; } = ConsoleColor.DarkRed;
    }

    public static class ConsoleExtensions {

        public static void ClearConsoleLine( this IQuestion question, int? line ) {

            if ( !line.HasValue ) { line = Console.CursorTop; }

            Console.SetCursorPosition( 0, Console.CursorTop );
            Console.Write( new string( ' ', Console.WindowWidth ) );
            Console.SetCursorPosition( 0, line.Value );
        }

    }
}