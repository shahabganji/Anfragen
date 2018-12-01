using System;

namespace Anfragen.Abstractions {

	public enum QuestionStates {
		Initilaized, Valid, Invalid, Finished
	}

	public abstract class Question : IQuestion {

		public string Hint { get; set; } = "";
		public string Answer { get; protected set; }
		public string Text { get; internal set; }

		public string ErrorMessage { get; set; } = "";

		public QuestionStates State { get; protected set; } = QuestionStates.Initilaized;
		public IQuestionnaire Questionnaire { get; set; }

		public IUserTerminal Terminal => this.Questionnaire.Terminal ?? null;

		private Func<IQuestion, bool> _Validator;

		public Question(string question, IQuestionnaire questionnaire = null) {

			this.Text = question ?? throw new ArgumentNullException($"{nameof(question)} cannot be null");

			this.Questionnaire = questionnaire;

		}

		// prints the question
		public virtual IQuestion Ask() {

			IUserTerminal terminal = this.Terminal;

			// 1. ask the question, simply buy just printing it
			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionIconColor;
			terminal.Printer.Write($"{this.Questionnaire.Settings.QuestionIcon} ");


			terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;
			terminal.Printer.Write(this.Text);

			// prints any hints available
			if (this.Hint.Trim().Length > 0) {
				terminal.ForegroundColor = this.Questionnaire.Settings.HintColor;
				terminal.Printer.Write($"( {this.Hint} )");
			}

			// a single space to seperate q/a
			terminal.Printer.Write(" ");


			// 2. wait for the user to give answer to the question
			this.TakeAnswer();

			terminal.ResetColor();

			return this;
		}

		public virtual void PrintResult() {
			this.Terminal.Printer.WriteLine($"{this.Text}: {this.Answer}");
		}

		public override string ToString() {
			return $"{this.Text}: {this.Answer}";
		}

		private Action<IQuestion> OnFinish;
		// the finishing touches of the question, for example rendering some extra texts
		public void Finish(Action<IQuestion> done = null) {

			if (done != null) {
				this.OnFinish = done;
				return;
			}

			done?.Invoke(this);
			this.State = QuestionStates.Finished;
		}

		public IQuestion Validator(Func<IQuestion, bool> validator, string errorMessage) {

			if (validator == null) {
				throw new ArgumentNullException($"{nameof(validator)} cannot be null");
			}

			if (errorMessage == null) {
				throw new ArgumentNullException($"{nameof(errorMessage) } cannot be null");
			}

			this.ErrorMessage = errorMessage;
			this._Validator = validator;
			return this;
		}

		protected virtual bool Validate() {

			bool result = false;

			//try {
			if (this._Validator == null) {
				// if no validator is provided then all the values will be considered true.
				this._Validator = exp => true;
				//throw new InvalidOperationException(
				//	$@"The validation function is 'null', you should provide validation function by calling {nameof(this.Validator)} method}.");
			}

			result = this._Validator(this); //.Invoke(this);

			this.State = result ? QuestionStates.Valid : QuestionStates.Invalid;

			if (result) {
				this.OnFinish?.Invoke(this);
			}

			//} catch(Exception ex) {

			//	//this.OnFinish?.Invoke(this, ex);
			//}

			return result;
		}

		// waits for the user to answer the question
		// Prints the validation errors
		protected virtual IQuestion PrintValidationErrors() {

			if (this.State == QuestionStates.Invalid && this.ErrorMessage.Trim().Length == 0) {
				throw new InvalidOperationException($"You must fill the property {nameof(this.ErrorMessage)}, when providing any validator. ");
			}

			this.Terminal.ForegroundColor = this.Questionnaire.Settings.ValidationIconColor;
			this.Terminal.Printer.Write(this.Questionnaire.Settings.ValidationIcon + " ");

			this.Terminal.ForegroundColor = this.Questionnaire.Settings.QuestionColor;

			this.Terminal.Printer.WriteLine(this.ErrorMessage);
			return this;
		}
		protected abstract IQuestion TakeAnswer();

		public object Clone() {
			return this.MemberwiseClone();
		}

	}

}