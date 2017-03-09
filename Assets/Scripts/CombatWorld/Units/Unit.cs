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
			GameController.instance.WaitForAction();
			currentNode.RemoveOccupant();
			node.SetOccupant(this);
			currentNode = node;
			transform.position = node.transform.position + new Vector3(0, 0.5f, 0);
			moved = true;
			Invoke("FinishedAction", 0.2f);
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
			GameController.instance.WaitForAction();
			entity.TakeDamage(new DamagePackage(damage, this, type));
			attacked = moved = true;
			Invoke("FinishedAction", 0.2f);
		}

		void RetaliationAttack(Entity entity) {
			GameController.instance.WaitForAction();
			entity.TakeDamage(new DamagePackage(damage, this, type, true));
		}

		public override void TakeDamage(DamagePackage damage) {
			base.TakeDamage(damage);
			if(!damage.WasRetaliation() && (health > 0 || DamageConstants.ALLOWRETALIATIONAFTERDEATH)) {
				RetaliationAttack(damage.GetSource());
			}
		}

		public int GetAttackValue() {
			return damage;
		}

		void FinishedAction() {
			GameController.instance.UnitMadeAction();
		}

		#region spawn

		public void SpawnEntity(Node node, Team team, CombatData data) {
			moveDistance = data.moveDistance;
			damage = data.attackValue;
			type = data.type;
			health = data.healthValue;
			this.team = team;
			currentNode = node;
			node.SetOccupant(this);
		}

		#endregion

	}
}