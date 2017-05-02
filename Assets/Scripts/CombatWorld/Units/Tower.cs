using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using CombatWorld.Map;
using System;

namespace CombatWorld.Units {
	public class Tower : MonoBehaviour, IEntity {

		private int health = DamageConstants.TOWERHP;
		private int maxHealth = DamageConstants.TOWERHP;
		[SerializeField]
		private Team team;
		[SerializeField]
		private Node currentNode;
		GameObject model;

		public HealthAttackVisualController healthIndicator;

		void Start() {
			model = transform.GetChild(0).gameObject;
			if (!currentNode.HasOccupant()) {
				currentNode.SetOccupant(this);
			}
			healthIndicator.Setup(health, 0);
			healthIndicator.UpdateHealthText(health + "/" + maxHealth);
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
			if (team == Team.AI) {
				healthIndicator.SummonPoint(false);
				DataGathering.Instance.KilledTower();
			}
			Instantiate (Resources.Load ("Art/3D/Explosion") as GameObject, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
			Instantiate (Resources.Load ("Art/3D/DeadTower") as GameObject, new Vector3(this.transform.position.x + 1, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
			model.SetActive(false);
			Destroy(gameObject,1);
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
			if(team == Team.AI) {
				DataGathering.Instance.AttackedTower();
			}
			damage.info.towerHit = true;
			health -= damage.CalculateDamageAgainst();
			healthIndicator.TookDamage(damage, (float)health/maxHealth);
			healthIndicator.UpdateHealthText(health + "/" + maxHealth);
			if (health <= 0) {
				damage.info.killHit = true;
				Die();
			}
		}

		void OnMouseOver() {
			if (CursorSingleton.instance != null)
				CursorSingleton.instance.SetCursor(currentNode.GetState());
		}

		void OnMouseExit() {
			if (CursorSingleton.instance != null)
				CursorSingleton.instance.SetCursor();
		}

		private void OnMouseDown() {
			currentNode.HandleInput();
		}
	}
}
