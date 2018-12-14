using Umfrage.Builders.Abstractions;

namespace Umfrage.Builders {
	public class QuestionBuilder : IQuestionBuilder {

		public IListQuestionBuilder List() {
			return new ListQuestionBuilder();
		}

		public ISimpleQuestionBuilder Simple() {
			return new SimpleQuestionBuilder();
		}
	}
}
