using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using CombatWorld.Map;
using System;

namespace CombatWorld.Units {
	public class Tower : MonoBehaviour, IEntity {

		[SerializeField]
		private int health;
		[SerializeField]
		private Team team;
		[SerializeField]
		private Node currentNode;

		void Start() {
			currentNode.SetOccupant(this);
			GameController.instance.AddTower(team);
		}

		public void Die() {
			GameController.instance.DestroyTower(team);
			currentNode.RemoveOccupant();
			Destroy(gameObject);
		}

		public int GetHealth() {
			return health;
		}

		public Node GetNode() {
			return currentNode;
		}

		public Team GetTeam() {
			return team;
		}

		public Transform GetTransform() {
			return transform;
		}

		public void TakeDamage(DamagePackage damage) {
			health -= damage.CalculateDamageAgainst();
			if (health <= 0) {
				Die();
			}
		}
	}
}
