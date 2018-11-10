using Anfragen.Interfaces;
using Anfragen.Extensions;

using System;
using System.Collections.Generic;
using System.IO;

namespace Anfragen.Implementations {

    public class Confirm : Question {

        public IList<string> PossibleAnswers { get; }

        public Confirm( string question, string[ ] possibleAnswers = null, IQuestionnaire questionnaire = null ) : base( question, questionnaire ) {

            this.Hint = "Yes/No";

            this.PossibleAnswers = possibleAnswers ?? new[ ] { "Yes", "No" };
        }

        protected override Question TakeAnswer( ) {

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
                terminal.ForegroundColor = this.Questionnaire.Settings.ValidationIconColor;
                terminal.Printer.Write( this.Questionnaire.Settings.ValidationIcon + " " );

                terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
                this.PrintValidationErrors( );

                // -1 beacause of readline
                Console.SetCursorPosition( left: cursorLeft, top: cursorTop - 1 );
                return this.TakeAnswer( );
            }

            terminal.ResetColor( );

            return this;
        }

    }

}