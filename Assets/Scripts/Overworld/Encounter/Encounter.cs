using UnityEngine;

namespace Overworld {
	public class Encounter : ContextInteraction {
		public MapTypes type = MapTypes.ANY;
		public int[] deckIDs = new int[1] { 0 };
		public int currencyForWinning = 25;

		public virtual void LoadCombat() {
			SceneHandler.instance.LoadScene(type, deckIDs[Random.Range(0, deckIDs.Length)], currencyForWinning);
		}
	}
}
