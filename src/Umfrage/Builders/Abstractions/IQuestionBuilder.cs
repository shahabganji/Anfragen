namespace Umfrage.Builders.Abstractions
{

	public interface IQuestionBuilder {
		ISimpleQuestionBuilder Simple();
		IListQuestionBuilder List();
	}
}
