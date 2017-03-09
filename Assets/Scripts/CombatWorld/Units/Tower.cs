using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class Tower : Entity {

		void Start() {
			currentNode.SetOccupant(this);
			GameController.instance.AddTower(team);
		}

		public override void Die() {
			GameController.instance.DestroyTower(team);
			base.Die();
		}
	}
}