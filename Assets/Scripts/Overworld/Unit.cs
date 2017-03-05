using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class Unit : MonoBehaviour, IUnit {
		public Texture2D icon;
		public int amount;
		private UnitDescription unit;
		
		public struct UnitDescription {
			public float damage;
			public float health;
			public float movementSpeed;
			public string story;
		}

		public UnitDescription GetDescription() {
			return unit;
		}
	}
}