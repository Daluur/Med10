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

		Entity target = null;

		public void Attack(Entity entity) {
			GameController.instance.WaitForAction();
			attacked = moved = true;
			target = entity;
			animHelp.Attack(entity.transform, DealDamage);
		}

		void RetaliationAttack(Entity entity) {
			GameController.instance.WaitForAction();
			target = entity;
			animHelp.Attack(entity.transform, DealRtaliationDamage);
		}

		void DealRtaliationDamage() {
			target.TakeDamage(new DamagePackage(damage, this, type, true));
			target = null;
			if(health <= 0) {
				Die();
				return;
			}
			FinishedAction();
		}

		void DealDamage() {
			target.TakeDamage(new DamagePackage(damage, this, type));
			target = null;
			FinishedAction();
		}

		DamagePackage damageIntake = null;

		public override void TakeDamage(DamagePackage damage) {
			damageIntake = damage;
			animHelp.TakeDamage(TookDamage);
		}

		void TookDamage() {
			health -= damageIntake.CalculateDamageAgainst(type);

			if(health <= 0) {
				//Died
				if (!damageIntake.WasRetaliation() && DamageConstants.ALLOWRETALIATIONAFTERDEATH) {
					RetaliationAttack(damageIntake.GetSource());
				}
				else {
					Die();
				}
			}
			else {
				//Didn't die
				if (!damageIntake.WasRetaliation()) {
					RetaliationAttack(damageIntake.GetSource());
				}
			}
			damageIntake = null;
		}

		public override void Die() {
			animHelp.Die(Death);
		}

		void Death() {
			FinishedAction();
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