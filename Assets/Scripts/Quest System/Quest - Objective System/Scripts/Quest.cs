using System.Collections.Generic;

namespace QuestSystem {	

	public class Quest {

		private IQuestText info;

		public IQuestText Info {
			get { return info; }
		}

		//objectives
		private List<IQuestObjective> objectives;
		//bonus objectives
		//collection objective
			//10 feathers
			//kill 4 enemies
		//location objective
			//a to b
			//timed version
		//rewards
		//events
			//on completion
			//on fail
			//on update

		private bool IsComplete() {
			
			for (int i = 0; i < objectives.Count; i++) {
				if (!objectives[i].IsComplete && !objectives[i].IsBonus) {
					return false;
				} 
				else {
					return true; //get reward
				}
			}
			return false;
		}

	}

}