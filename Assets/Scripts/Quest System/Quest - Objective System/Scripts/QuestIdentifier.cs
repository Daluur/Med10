using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {

	public class QuestIdentifier : IQuestIdentifier {

		private string questID;
		private string chainQuest;
		private string sourceID;

		public string QuestID {
			get {
				return questID;
			}
		}

		public string SourceID {
			get {
				return sourceID;
			}
		}

		public string ChainQuest {
			get {
				return chainQuest;
			}
		}
	}
}