namespace QuestSystem {

	public interface IQuestIdentifier {

		string QuestID { get; }
		string SourceID { get; }
		string ChainQuest { get; }
	}
}