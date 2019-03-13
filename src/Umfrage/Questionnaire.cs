using Umfrage.Abstractions;
using Umfrage.Builders;
using Umfrage.Builders.Abstractions;
using Umfrage.Implementations;
using System;

namespace Umfrage {

	public class Questionnaire : IQuestionnaire {

		#region fields: 

		private int currentStep = 0;
		private IBranch currentBranch = null;

		private bool hasStarted = false;
		private bool branchSwitched = false;

		private List<IBranch> _branches;
		private List<IQuestion> _questions;
		private List<IQuestion> _processedQuestions;

		#endregion

		#region properties : 

		public bool CanProceed => this.NextQuestion != null;

		public IQuestionBuilder Builder { get; }

		public IUserTerminal Terminal { get; private set; }

		public IEnumerable<IBranch> Branches => this._branches;
		public IEnumerable<IQuestion> Questions => this._questions;

		public IEnumerable<IQuestion> ProcessedQuestions => this._processedQuestions;


		private int Count => currentBranch?.Questions.Count() ?? this._questions.Count;

		public IQuestion PreviousQuestion {
			get {
				// you are at hte beginning of the list , so no previous questions
				if (this.currentStep == 0) {
					return null;
				}

				return this.currentBranch == null ? this._questions[ this.currentStep - 1 ] : this.currentBranch.Questions.ToList()[ this.currentStep - 1 ];
			}
		}

		public IQuestion CurrentQuestion {
			get {
				// this.should never happen
				if (this.currentStep >= this.Count) {
					return null;
				}

				return this.currentBranch == null ? this._questions[ this.currentStep ] : this.currentBranch.Questions.ToList()[ this.currentStep ];
			}
		}

		public IQuestion NextQuestion {
			get {
				// you are at the last question so no more questions exists
				if (this.currentStep + 1 == this.Count) {
					return null;
				}

				try {
					return this.currentBranch == null ? this._questions[ this.currentStep + 1 ] : this.currentBranch.Questions.ToList()[ this.currentStep + 1 ];
				} catch {
					return null;
				}

			}
		}

		public QuestionnaireSetting Settings { get; set; }

		#endregion

		public Questionnaire(IUserTerminal userConsole = null, QuestionnaireSetting settings = null , IQuestionBuilder builder = null) {

			this.Builder = builder ?? new QuestionBuilder();
			this.Terminal = userConsole ?? new UserTerminal();
			this.Settings = settings ?? new QuestionnaireSetting();
			
			this._branches = new List<IBranch>();
			this._questions = new List<IQuestion>();
			this._processedQuestions = new List<IQuestion>();


		}

		private void AskQuestionWaitAnswer() {

			IQuestion question = this.CurrentQuestion;

			question.Ask();

			this._processedQuestions.Add(this.CurrentQuestion);
		}

		private IBranch FindBranch(string branchName) {
			IBranch branch = this._branches.SingleOrDefault( b => b.Name == branchName );

			if (branch == null) {
				throw new InvalidOperationException($"The branch '{branchName}' does not exists");
			}

			return branch;
		}

		public IQuestionnaire Start() {

			if (this._questions.Count == 0) {
				throw new InvalidOperationException("There is no questions in this questionnaire, please first add some");
			}

			if (this.hasStarted) {
				throw new InvalidOperationException("You cannot start a questionnaire more than once.");
			}

			this.hasStarted = true;

			this.Terminal.Printer.WriteLine(this.Settings.WelcomeMessage);

			this.AskQuestionWaitAnswer();

			return this;
		}

		public IQuestionnaire GoToBranch(string branchName) {

			if (this.CurrentQuestion.State != QuestionStates.Finished) {
				throw new InvalidOperationException("You cannot go to next step, unless you finish the previous question");
			}

			IBranch branch = this.FindBranch( branchName );

			if (branch.Questions.Count() == 0) {
				throw new InvalidOperationException("You can not switch to a branch without questions");
			}

			// Coz after this switching the user must call 
			// "Next" method to activate the first question in the branch
			this.currentStep = 0;
			this.currentBranch = branch;
			this.branchSwitched = true;

			return this;
		}

		public IQuestionnaire Next() {

			if (!this.hasStarted) {
				throw new InvalidOperationException("You have not strted the questionnaire yet");
			}

			// do not proceed if the currently asked question is not in Valid state
			if (this.CurrentQuestion.State != QuestionStates.Valid) {
				return this;
			}

			// checks to see whether we have just switched to the new branch, 
			// if so, there is no need to prceed as the pointer already points to the first question
			if (this.branchSwitched) {
				this.branchSwitched = false;
			} else {
				// proceeds the counter
				this.currentStep++;
			}

			if (this.currentStep == this.Count) {
				// to cancel the effect of previous addition to the current step;
				this.currentStep--;
				throw new InvalidOperationException("Your at the end of the questionnaire, there is no more questions.");
			}

			this.AskQuestionWaitAnswer();

			return this;
		}

		public IQuestionnaire GotToStep(int step, string branchName = null) {

			if (this.CurrentQuestion.State != QuestionStates.Finished) {
				throw new InvalidOperationException("You cannot go to next step, unless you finish the previous question");
			}

			// check wheather the user wants to swithc to a step in another branch or not
			IBranch branch = branchName != null ? this.FindBranch( branchName ) : null;

			// the step should not be more than the length of questions in the target branch
			int count = branch != null ? branch.Questions.Count( ) : this._questions.Count;

			// if step is out of range then an IndexOutOfRangeException will be thrown
			if (step < 0 || step > count) {
				throw new IndexOutOfRangeException($@"{nameof(step)} should be between 0 and {count}, 
                                                            number of questions in your branch.");
			}

			// Set current branch and current step
			this.currentBranch = branch;
			this.currentStep = step - 1;

			return this;

		}

		public IQuestionnaire Add(IBranch branch) {
			this._branches.Add(branch);

			return this;
		}

		public IQuestionnaire Add(IQuestion question, IBranch branch = null, bool here = false) {

			// the question already blongs to a quesstionnnaire
			if( question.Questionnaire != null) {
				throw new InvalidOperationException($"The {nameof(question)} already belongs to a questionnaire, cannot add it.");
			}

			question.Questionnaire = this;

			if (branch == null) {

				if (here) {
					this._questions.Insert(this.currentStep + 1, question);
				} else {
					this._questions.Add(question);
				}
			} else {

				if (here) {
					branch.Add(question, this.currentStep + 1);
				} else {
					branch.Add(question);
				}

			}
			return this;
		}

		public void End() {
			this.Terminal.Printer.WriteLine();
			this.Terminal.Printer.Write("---- END OF Questionnaire ----");
			this.Terminal.Printer.WriteLine();
		}

		public IQuestionnaire Prev() {

			if (this.currentStep == 0) {
				throw new InvalidOperationException("You are at the beginning of the questionnaire, there is no previous question.");
			}

			this.currentStep--;

			this.AskQuestionWaitAnswer();

			return this;

		}

	}
}
