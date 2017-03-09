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

		AnimationHelper animHelp;

		void Start() {
			animHelp = GetComponentInChildren<AnimationHelper>();
		}

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
			attacked = moved = true;
			animHelp.Attack(entity.transform, DealDamage, new DamagePackage(damage, this, type), entity);
		}

		void RetaliationAttack(Entity entity) {
			GameController.instance.WaitForAction();
			animHelp.Attack(entity.transform, DealDamage, new DamagePackage(damage, this, type, true), entity);
		}

		void DealDamage(DamagePackage damage, Entity entity) {
			entity.TakeDamage(damage);
			FinishedAction();
		}

		public override void TakeDamage(DamagePackage damage) {
			animHelp.TakeDamage(TookDamage, damage);
			base.TakeDamage(damage);
		}

		void TookDamage(DamagePackage damage) {
			if (!damage.WasRetaliation() && (health > 0 || DamageConstants.ALLOWRETALIATIONAFTERDEATH)) {
				RetaliationAttack(damage.GetSource());
			}
		}

		public override void Die() {
			animHelp.Die(Death);
			base.Die();
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