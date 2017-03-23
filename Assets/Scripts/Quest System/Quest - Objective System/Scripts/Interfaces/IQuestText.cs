namespace QuestSystem {

	public interface IQuestText {

		string Title { get; }
		string Description { get; }
		string Hint { get; }
		string Dialogue { get; }
	}
}