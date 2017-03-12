using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class Entity : MonoBehaviour {

		[SerializeField]
		protected int health;
		[SerializeField]
		protected Team team;
		[SerializeField]
		protected Node currentNode;

		public ElementalTypes type;

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

		public virtual void Die() {
			GameController.instance.UnitDied(team);
			currentNode.RemoveOccupant();
			GameController.instance.UnitMadeAction();
			Destroy(gameObject);
		}

		public int GetHealth() {
			return health;
		}
	}
}