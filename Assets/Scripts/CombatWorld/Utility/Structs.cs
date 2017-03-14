using UnityEngine;

namespace CombatWorld.Utility {

	[System.Serializable]
	public struct CombatData {
		public int moveDistance;
		public int attackValue;
		public int healthValue;
		public int cost;
		public GameObject model;
		public ElementalTypes type;

		public ElementalTypes GetTypeFromString(string type) {
			switch (type) {
				case "fire_unit":
					return ElementalTypes.Fire;
				case "water_unit":
					return ElementalTypes.Water;
				case "lightning_unit":
					return ElementalTypes.Lightning;
				case "nature_unit":
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
