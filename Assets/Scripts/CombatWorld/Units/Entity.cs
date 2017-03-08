using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class Entity : MonoBehaviour {

		protected int health;
		protected ElementalTypes type;

		protected Team team;
		protected Node currentNode;

		public Team GetTeam() {
			return team;
		}

		public Node GetNode() {
			return currentNode;
		}

		public virtual void TakeDamage(DamagePackage damage) {
			health -= damage.CalculateDamageAgainst(type);
			if(health <= 0) {
				Die();
			}
		}

		void Die() {
			currentNode.RemoveOccupant();
			Destroy(gameObject);
		}
	}
}