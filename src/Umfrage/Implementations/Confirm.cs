using Umfrage.Extensions;
using Umfrage.Abstractions;

using System;
using System.Collections.Generic;

namespace Umfrage.Implementations {

	public class Confirm : Question {

        public IList<string> PossibleAnswers { get; }

        public Confirm( string question, string hint = "y/n" ,  string[ ] possibleAnswers = null, IQuestionnaire questionnaire = null ) : base( question, questionnaire ) {

            this.Hint = hint;

            this.PossibleAnswers = possibleAnswers ?? new[ ] { "y", "n" };
        }

        protected override IQuestion TakeAnswer( ) {

            IUserTerminal terminal = this.Terminal;

            // this should be before ReadLine, 
            // because ReadLine will eliminate the current Lef position and reset it to 0
            int cursorLeft = Console.CursorLeft;

            // always set the color of terminal to AnswerColor
            terminal.ForegroundColor = this.Questionnaire.Settings.AnswerColor;
            this.Answer = terminal.Scanner.ReadLine( );

            // this should be after ReadLine because 
            // before the user enters the input he/she might resize the console, 
            // hence the top will change 
            int cursorTop = Console.CursorTop;

            bool result = this.Validate( );
            this.State = result ? QuestionStates.Valid : QuestionStates.Invalid;

            if ( result ) {
                this.ClearLine( cursorTop );
            } else {
                this.PrintValidationErrors( );

				// -1 beacause of readline
				var line = cursorTop - 1;

				this.ClearAnswer(line: line);
                Console.SetCursorPosition( left: cursorLeft, top: line );
                return this.TakeAnswer( );
            }

            terminal.ResetColor( );

            return this;
        }

    }
}
