using UnityEngine;
using System;
using System.Collections.Generic;

namespace CombatWorld.Utility {

	[Serializable]
	public struct CombatData {
		public string name;
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

		public string GetStringFromType(ElementalTypes type) {
			switch (type) {
				case ElementalTypes.Fire:
					return "Fire";
				case ElementalTypes.Water:
					return "Water";
				case ElementalTypes.Nature:
					return "Nature";
				case ElementalTypes.Lightning:
					return "Lightning";
				case ElementalTypes.NONE:
				default:
					return "Normal";
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
			name = item.Title;
			type = GetTypeFromString(item.Type);
		}
	}

	[Serializable]
	public struct SummonData {
		public GameObject Unit;
		public int Cost;
		public CombatData data;
	}

	public static class Utility {
		public static ElementalTypes GetElementalTypeFromID(int id) {
			switch (id) {
				case 0:
				case 1:
					return ElementalTypes.Fire;
				case 2:
				case 3:
					return ElementalTypes.Water;
				case 4:
				case 5:
					return ElementalTypes.Lightning;
				case 6:
				case 7:
					return ElementalTypes.Nature;
				default:
					return ElementalTypes.NONE;
			}
		}

		public static bool GetIsShadowFromID(int id) {
			return id == 8 || id == 9;
		}

		public static bool GetIsStoneFromID(int id) {
			return id == 10 || id == 11;
		}
	}

}
[Serializable]
public struct DeckData {
	public string deckName, difficulty;
	public int id;
	public int[] unitIDs;
	public string type1;
	public string type2;
}