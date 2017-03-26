using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;

namespace CombatWorld.AI {
	public class AIUnit {

		public AIUnit(Unit unit, Pathfinding pathfinding) {
			myUnit = unit;
			this.pathFinding = pathfinding;
		}

		public List<AITask> possibleTasks = new List<AITask>();

		public AITask taskToDo;

		public Pathfinding pathFinding = new Pathfinding();

		public Unit myUnit;

		public void PerformTask(AITask task) {
			AITaskImplementations.DoAIAction(this, task);
		}

		public void ClearTasks() {
			possibleTasks.Clear();
			taskToDo = null;
		}

		public void SetTaskToDo(AITask task) {
			taskToDo = task;
		}

	}
}