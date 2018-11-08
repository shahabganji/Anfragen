using System;
using System.Collections.Generic;
using System.Linq;
using Anfragen.Interfaces;

namespace Anfragen {

    public class Questionnaire : IQuestionnaire {

        #region fields: 

        public readonly IPrinter printer;

        private int currentStep = 0;
        private IBranch currentBranch = null;

        private bool hasStarted = false;
        private bool branchSwitched = false;

        private List<IQuestion> _questions;
        private List<IBranch> _branches;

        #endregion

        #region properties : 

        public IEnumerable<IQuestion> Questions => this._questions;

        public IEnumerable<IBranch> Branches => this._branches;

        private int Count => this.currentBranch != null ? this.currentBranch.Questions.Count( ) : this._questions.Count;

        public IQuestion PreviousQuestion {
            get {
                // you are at hte beginning of the list , so no previous questions
                if ( this.currentStep == 0 ) {
                    return null;
                }

                return this.currentBranch == null ? this._questions[ this.currentStep - 1 ] : this.currentBranch.Questions.ToList( )[ this.currentStep - 1 ];
            }
        }

        public IQuestion CurrentQuestion {
            get {
                // this.should never happen
                if ( this.currentStep >= this.Count ) {
                    return null;
                }

                return this.currentBranch == null ? this._questions[ this.currentStep ] : this.currentBranch.Questions.ToList( )[ this.currentStep ];
            }
        }

        public IQuestion NextQuestion {
            get {
                // you are at the last question so no more questions exists
                if ( this.currentStep + 1 == this.Count )
                    return null;

                return this.currentBranch == null ? this._questions[ this.currentStep + 1 ] : this.currentBranch.Questions.ToList( )[ this.currentStep + 1 ];

            }
        }

        #endregion

        public Questionnaire( IPrinter printer ) {

            this.printer = printer;

            this._branches = new List<IBranch>( );
            this._questions = new List<IQuestion>( );

        }

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

            IBranch branch = FindBranch( branchName );

            if ( branch.Questions.Count( ) == 0 ) {
                throw new InvalidOperationException( "You can not switch to a branch without questions" );
            }

            // Coz after this switching the user must call 
            // "GoToNextStep" method to activate the first question in the branch
            this.currentStep = 0;
            this.currentBranch = branch;
            this.branchSwitched = true;

            return this;
        }

        private IBranch FindBranch( string branchName ) {
            var branch = this._branches.SingleOrDefault( b => b.Name == branchName );

            if ( branch == null ) {
                throw new InvalidOperationException( $"The branch '{branchName}' does not exists" );
            }

            return branch;
        }

        public IQuestionnaire GoToNextStep( ) {

            if ( !hasStarted ) {
                throw new InvalidOperationException( "You have not strted the questionnaire yet" );
            }

            // checks to see whether we have just switched to the new branch, 
            // if so, there is no need to prceed as the pointer already points to the first question
            if ( this.branchSwitched ) {
                this.branchSwitched = false;
            } else {
                // proceeds the counter
                this.currentStep++;
            }

            if ( this.currentStep == this.Count ) {
                // to cancel the effect of previous addition to the current step;
                this.currentStep--;
                throw new InvalidOperationException( "Your at the end of the questionnaire, there is no more questions." );
            }

            var question = this.CurrentQuestion;

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;
        }

        public IQuestionnaire GotToStep( int step, string branchName = null ) {

            // check wheather the user wants to swithc to a step in another branch or not
            var branch = branchName != null ? this.FindBranch( branchName ) : null;

            // the step should not be more than the length of questions in the target branch
            var count = branch != null ? branch.Questions.Count( ) : this._questions.Count;

            // if step is out of range then an IndexOutOfRangeException will be thrown
            if ( step < 0 || step > count ) {
                throw new IndexOutOfRangeException( $@"{nameof( step )} should be between 0 and {count}, 
                                                            number of questions in your branch." );
            }

            // Set current branch and current step
            this.currentBranch = branch;
            this.currentStep = step - 1;

            return this;

        }

        public IQuestionnaire Add( IBranch branch ) {
            this._branches.Add( branch );

            return this;
        }

        public IQuestionnaire Add( IQuestion question, IBranch branch = null, bool here = false ) {

            if ( branch == null ) {

                if ( here ) {
                    this._questions.Insert( this.currentStep + 1, question );
                } else {
                    this._questions.Add( question );
                }
            } else {

                if ( here ) {
                    branch.Add( question, this.currentStep + 1 );
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

        public IQuestionnaire GoToPreviousStep( ) {

            throw new NotImplementedException( );

            if ( this.currentStep == 0 ) {
                throw new InvalidOperationException( "Your at the beginning of the questionnaire, there is no previous question." );
            }

            this.currentStep--;

            var question = this._questions[ currentStep ];

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;

        }


    }
}