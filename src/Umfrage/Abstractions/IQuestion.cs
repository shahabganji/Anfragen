using System;

namespace Umfrage.Abstractions
{
	public interface IQuestion : ICloneable {

		string Hint { get; set; }
		string Answer { get;}
		string DefaultAnswer { get; set; }
		string Text { get; }

		string ErrorMessage { get; set; }

		QuestionStates State { get; }
		IQuestionnaire Questionnaire { get; set; }

		IUserTerminal Terminal { get; }

		IQuestion Ask();

		void PrintResult();

		string ToString();

		void Finish(Action<IQuestion> done = null);

		IQuestion Validator(Func<IQuestion, bool> validator, string errorMessage);

	}
}
