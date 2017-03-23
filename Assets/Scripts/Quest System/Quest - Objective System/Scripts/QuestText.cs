namespace QuestSystem {

	public class QuestText : IQuestText {

		private string title;
		private string description;
		private string dialog;
		private string hint;

		public string Title {
			get {
				return title;
			}
		}

		public string Description {
			get {
				return description;
			}
		}

		public string Hint {
			get {
				return hint;
			}
		}

		public string Dialogue {
			get {
				return dialog;
			}
		}
	}
}