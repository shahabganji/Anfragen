using System.Collections.Generic;
using Anfragen.Interfaces;
using System.Linq;
using System;

namespace Anfragen {

    public class Questionnaire : IQuestionnaire {

        public readonly IPrinter printer;

        private List<IQuestion> _questions;

        public IEnumerable<IQuestion> Questions => this._questions;

        private List<IBranch> _branches;
        public IEnumerable<IBranch> Branches => this._branches;

        private int Count => this._currentBranch != null ? this._currentBranch.Questions.Count( ) : this._questions.Count;


        public IQuestion PreviousQuestion {
            get {
                // you are at hte beginning of the list , so no previous questions
                if ( this._counter == 0 ) {
                    return null;
                }

                return this._currentBranch == null ? this._questions[ this._counter - 1 ] : this._currentBranch.Questions.ToList( )[ this._counter - 1 ];
            }
        }

        public IQuestion CurrentQuestion {
            get {
                // this.should never happen
                if ( this._counter >= this.Count ) {
                    return null;
                }

                return this._currentBranch == null ? this._questions[ this._counter ] : this._currentBranch.Questions.ToList( )[ this._counter ];
            }
        }

        public IQuestion NextQuestion {
            get {
                // you are at the last question so no more questions exists
                if ( this._counter + 1 == this.Count )
                    return null;

                return this._currentBranch == null ? this._questions[ this._counter + 1 ] : this._currentBranch.Questions.ToList( )[ this._counter + 1 ];

            }
        }



        public Questionnaire( IPrinter printer ) {

            this.printer = printer;

            this._branches = new List<IBranch>( );
            this._questions = new List<IQuestion>( );

        }

        private int _counter = 0;
        private bool hasStarted = false;
        private bool isInNewBranch = false;
        private IBranch _currentBranch = null;

        public IQuestionnaire Start( ) {

            if ( this.hasStarted ) {
                throw new InvalidOperationException( "You cannot start a questionnaire more than once." );
            }

            this.hasStarted = true;

            var question = this.CurrentQuestion;

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;
        }


        public IQuestionnaire GoToBranch( string branchName ) {

            var branch = this._branches.SingleOrDefault( b => b.Name == branchName );

            if ( branch == null ) {
                throw new InvalidOperationException( $"The branch '{branchName}' does not exists" );
            }

            this._counter = 0; // coz after this switching the user must call GoToNextStep method
            this._currentBranch = branch;
            this.isInNewBranch = true;

            return this;
        }

        public IQuestionnaire GoToNextStep( ) {

            if ( !hasStarted ) {
                throw new InvalidOperationException( "You have not strted the questionnaire yet" );
            }

            // checks to see whether we have just switched to the new branch, 
            // if so, there is no need to prceed as the pointer already points to the first question
            if ( this.isInNewBranch ) {
                this.isInNewBranch = false;
            } else {
                // proceeds the counter
                this._counter++;
            }

            if ( this._counter == this.Count ) {
                throw new InvalidOperationException( "Your at the end of the questionnaire, there is no more questions." );
            }

            var question = this.CurrentQuestion;

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;
        }

        public IQuestionnaire GoToPreviousStep( ) {

            throw new NotImplementedException( );

            if ( this._counter == 0 ) {
                throw new InvalidOperationException( "Your at the beginning of the questionnaire, there is no previous question." );
            }

            this._counter--;

            var question = this._questions[ _counter ];

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;

        }

        public IQuestionnaire GotToStep( string branchName, int step ) {

            throw new NotImplementedException( );
        }

        public IQuestionnaire Add( IBranch branch ) {
            this._branches.Add( branch );

            return this;
        }

        public IQuestionnaire Add( IQuestion question, IBranch branch = null, bool here = false ) {

            if ( branch == null ) {

                if ( here ) {
                    this._questions.Insert( this._counter + 1, question );
                } else {
                    this._questions.Add( question );
                }
            } else {

                if ( here ) {
                    branch.Add( question, this._counter + 1 );
                } else {
                    branch.Add( question );
                }

            }
            return this;
        }

        public void End( ) {
            printer.AddNewLine( );
            printer.Print( "---- END OF Questionnaire ----" );
            printer.AddNewLine( );
        }
    }
}