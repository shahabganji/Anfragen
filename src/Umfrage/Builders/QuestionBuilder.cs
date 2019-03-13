using Umfrage.Builders.Abstractions;

namespace Umfrage.Builders
{
	public class QuestionBuilder : IQuestionBuilder {

		public IListQuestionBuilder List() {
			return new ListQuestionBuilder(this);
		}

		public ISimpleQuestionBuilder Simple() {
			return new SimpleQuestionBuilder(this);
		}
	}
}
