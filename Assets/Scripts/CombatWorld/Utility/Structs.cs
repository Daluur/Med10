using UnityEngine;

namespace CombatWorld.Utility {

	[System.Serializable]
	public struct CombatData {
		public int moveDistance;
		public int attackValue;
		public int healthValue;
		public ElementalTypes type;
	}

	[System.Serializable]
	public struct SummonData {
		public GameObject Unit;
		public int Cost;
		public CombatData data;
	}

}
