using UnityEngine;

namespace CombatWorld.Utility {

	[System.Serializable]
	public struct CombatData {
		public int moveDistance;
		public int attackValue;
		public int healthValue;
		public int cost;
		public bool shadow;
		public bool stone;
		public GameObject model;
		public ElementalTypes type;

		public ElementalTypes GetTypeFromString(string type) {
			switch (type) {
				case "Fire":
					return ElementalTypes.Fire;
				case "Water":
					return ElementalTypes.Water;
				case "Lightning":
					return ElementalTypes.Lightning;
				case "Nature":
					return ElementalTypes.Nature;
				default:
					return ElementalTypes.NONE;
			}
		}

		public CombatData(Item item) {
			attackValue = item.Attack;
			healthValue = item.Health;
			moveDistance = item.Moves;
			type = ElementalTypes.NONE;
			cost = item.SummonCost;
			shadow = item.Shadow;
			stone = item.Stone;
			model = item.Model;
			type = GetTypeFromString(item.Type);
		}
	}

	[System.Serializable]
	public struct SummonData {
		public GameObject Unit;
		public int Cost;
		public CombatData data;
	}

}
