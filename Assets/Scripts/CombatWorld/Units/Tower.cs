using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using CombatWorld.Map;
using System;

namespace CombatWorld.Units {
	public class Tower : MonoBehaviour, IEntity {

		private int health = 15;
		[SerializeField]
		private Team team;
		[SerializeField]
		private Node currentNode;

		void Start() {
			if (!currentNode.HasOccupant()) {
				currentNode.SetOccupant(this);
			}
			GameController.instance.AddTower(team);
		}

		public void SetCurrentNode(Node node) {
			currentNode = node;
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

		public void SetTeam(Team newTeam) {
			team = newTeam;
		}

		public Transform GetTransform() {
			return transform;
		}

		public void TakeDamage(DamagePackage damage) {
			health -= damage.CalculateDamageAgainst();
			GameController.instance.UnitMadeAction();
			if (health <= 0) {
				Die();
			}
		}
	}
}
