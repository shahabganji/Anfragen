namespace Anfragen.Implementations {
	public class ListOption : IOption {
		public string Text { get; }
		public bool Selected { get; set; }

		public ListOption(string option, bool selected = false) {
			this.Text = option;
			this.Selected = selected;
		}

		public override string ToString() {
			return this.Text;
		}
	}
}
