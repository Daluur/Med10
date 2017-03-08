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

		public void Attack(Entity entity, bool retaliation = false) {
			entity.TakeDamage(new DamagePackage(damage, this, type, retaliation));
			attacked = moved = true;
			GameController.instance.UnitMadeAction();
		}

		public override void TakeDamage(DamagePackage damage) {
			base.TakeDamage(damage);
			if(!damage.WasRetaliation() && (health > 0 || DamageConstants.ALLOWRETALIATIONAFTERDEATH)) {
				Debug.Log("Made retaliation attack.");
				Attack(damage.GetSource(), true);
			}
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