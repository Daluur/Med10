using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class Unit : Entity {

		int moveDistance;

		int damage;

		bool moved = true;
		bool attacked = true;

		public void Move(Node node) {
			currentNode.RemoveOccupant();
			node.SetOccupant(this);
			currentNode = node;
			GameController.instance.UnitMadeAction();
			transform.position = node.transform.position + new Vector3(0, 0.5f, 0);
			moved = true;
		}

		public bool CanMove() {
			return !moved;
		}

		public bool CanAttack() {
			return !attacked;
		}

		public int GetMoveDistance() {
			return moveDistance;
		}

		public void newTurn() {
			moved = false;
			attacked = false;
		}

		public void Attack(Entity entity) {
			entity.TakeDamage(new DamagePackage(damage));
			attacked = moved = true;
			GameController.instance.UnitMadeAction();
		}

		#region spawn

		public void SpawnEntity(Node node, Team team) {
			moveDistance = 2;
			damage = 2;
			type = ElementalTypes.NONE;
			health = 4;
			this.team = team;
			currentNode = node;
			node.SetOccupant(this);
		}

		#endregion

	}
}