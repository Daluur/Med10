using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class Unit : MonoBehaviour, IEntity {

		private int health;
		private Team team;
		private Node currentNode;

		private ElementalTypes type;

		private int moveDistance;

		private int damage;

		private bool moved = true;
		private bool attacked = true;

		private AnimationHandler animHelp;

		void Start() {
			animHelp = GetComponentInChildren<AnimationHandler>();
		}

		public void Move(Node node) {
			GameController.instance.WaitForAction();
			currentNode.RemoveOccupant();
			node.SetOccupant(this);
			currentNode = node;
			transform.position = node.transform.position + new Vector3(0, 0.5f, 0);
			moved = true;
			//Misses animation.
			//should not use Invoke.
			Invoke("FinishedAction", 0.2f);
		}

		#region Getters

		public int GetHealth() {
			return health;
		}

		public Node GetNode() {
			return currentNode;
		}

		public Team GetTeam() {
			return team;
		}

		public bool CanMove() {
			return !moved;
		}

		public int GetMoveDistance() {
			return moveDistance;
		}

		public bool CanAttack() {
			return !attacked;
		}

		public int GetAttackValue() {
			return damage;
		}

		public Transform GetTransform() {
			return transform;
		}

		#endregion

		public void NewTurn() {
			moved = attacked = false;
		}

		public void Attack(IEntity target) {
			attacked = moved = true;
			DealDamage(target, new DamagePackage(damage, this, type));
		}

		void RetaliationAttack(IEntity target) {
			DealDamage(target, new DamagePackage(damage, this, type, true));
		}

		void DealDamage(IEntity target, DamagePackage damage) {
			GameController.instance.WaitForAction();
			if (damage.WasRetaliation() && health <= 0) {
				animHelp.Attack(target.GetTransform(), Die);
			}
			else {
				animHelp.Attack(target.GetTransform(), FinishedAction);
			}
			target.TakeDamage(damage);
		}

		DamagePackage damageIntake = null;

		public void TakeDamage(DamagePackage damage) {
			damageIntake = damage;
			health -= damageIntake.CalculateDamageAgainst(type);
			animHelp.TakeDamage(TookDamage);
		}

		void TookDamage() {
			if(health <= 0) {
				//Die unless it can do retaliation.
				if (!damageIntake.WasRetaliation() && DamageConstants.ALLOWRETALIATIONAFTERDEATH) {
					RetaliationAttack(damageIntake.GetSource());
				}
				else {
					Die();
				}
			}
			//Didn't die, retaliates, if attack was not retaliation (no infinite loops ;) )
			else if(!damageIntake.WasRetaliation()) {
				RetaliationAttack(damageIntake.GetSource());
			}
			damageIntake = null;
		}

		public void Die() {
			animHelp.Die(Death);
		}

		void Death() {
			GameController.instance.UnitDied(team, currentNode);
			currentNode.RemoveOccupant();
			FinishedAction();
			Destroy(gameObject);
		}

		void FinishedAction() {
			GameController.instance.UnitMadeAction();
		}

		public void SpawnEntity(Node node, Team team, CombatData data) {
			moveDistance = data.moveDistance;
			damage = data.attackValue;
			type = data.type;
			health = data.healthValue;
			this.team = team;
			currentNode = node;
			node.SetOccupant(this);
		}
	}
}
