namespace Umfrage.Abstractions
{
	public interface IOption {
		string Text { get; }
		bool Selected { get; set; }
	}
}
