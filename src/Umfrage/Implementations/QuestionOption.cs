using Umfrage.Abstractions;

namespace Umfrage.Implementations
{
	public class QuestionOption : IOption {
		public string Text { get; }
		public bool Selected { get; set; }

		public QuestionOption(string option, bool selected = false) {
			this.Text = option;
			this.Selected = selected;
		}

		public override string ToString() {
			return this.Text;
		}
	}
}
