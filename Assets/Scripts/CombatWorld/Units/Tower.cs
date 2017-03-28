using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using CombatWorld.Map;
using System;

namespace CombatWorld.Units {
	public class Tower : MonoBehaviour, IEntity {

		private int health = DamageConstants.TOWERHP;
		[SerializeField]
		private Team team;
		[SerializeField]
		private Node currentNode;

		public HealthAttackVisualController healthIndicator;

		void Start() {
			if (!currentNode.HasOccupant()) {
				currentNode.SetOccupant(this);
			}
			healthIndicator.Setup(health, 0);
			GameController.instance.AddTower(team);
		}

		public void SetCurrentNode(Node node) {
			currentNode = node;
		}

		public void Die() {
			GameController.instance.DestroyTower(team);
			foreach (Node node in currentNode.GetNeighbours()) {
				node.neighbours.Remove(currentNode);
			}
			currentNode.neighbours.Clear();
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
			healthIndicator.TookDamage(damage, health);
			if (health <= 0) {
				Die();
			}
		}
	}
}
