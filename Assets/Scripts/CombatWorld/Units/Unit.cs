using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;
using UnityEngine.EventSystems;

namespace CombatWorld.Units {
	public class Unit : MonoBehaviour, IEntity {

		[Tooltip("Name of the trigger. Useable: \nMelee Right Attack 01 \nMelee Right Attack 02 \nMelee Right Attack 03 \nMelee Left Attack 01 \nLeft Punch Attack \nRight Punch Attack \nProjectile Right Attack 01 \nCrossbow Attack \nCast Spell 01 \nCast Spell 02")]
		public string attackName = "Melee Right Attack 01";
		public GameObject projectile;

		private bool shadowUnit = false;
		private bool stoneUnit = false;
		bool turnedToStone = false;

		private int health;
		private Team team;
		private Node currentNode;

		private ElementalTypes type;

		private int moveDistance;

		private int damage;

		private bool moved = true;
		private bool attacked = true;

		private Vector3 defaultFaceDirection;

		public CombatData data;

		private float moveSpeed = 12.5f;

		private AnimationHandler animHelp;

		[HideInInspector]
		public ParticleSystem particles;

		public HealthAttackVisualController healthIndicator;

		void Start() {
			animHelp = GetComponentInChildren<AnimationHandler>().Setup(attackName, shadowUnit);
		}

		public void Move(List<Node> node) {
			GameController.instance.AddWaitForUnit(this);
			currentNode.RemoveOccupant();
			currentNode = node[0];
			currentNode.SetOccupant(this);
			StartCoroutine(MoveTo(node));
			moved = true;
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

		public ElementalTypes GetElementalType() {
			return type;
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

		public bool IsShadowUnit() {
			return shadowUnit;
		}

		public bool IsStoneUnit() {
			return stoneUnit;
		}

		#endregion

		public void NewTurn() {
			if (turnedToStone) {
				return;
			}
			moved = attacked = false;
		}

		IEntity target;
		DamagePackage damagePack;

		public void Attack(IEntity target) {
			GameController.instance.AddWaitForUnit(this);
			this.target = target;
			attacked = moved = true;
			damagePack = new DamagePackage(damage, this, type);
			DealDamage();
		}

		void RetaliationAttack(IEntity target) {
			this.target = target;
			damagePack = new DamagePackage(damage, this, type, true);
			DealDamage();
		}

		void DealDamage() {
			transform.LookAt(target.GetTransform());
			animHelp.Attack(SpawnProjectile);
		}

		void SpawnProjectile() {
			Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>().Setup(target.GetTransform(), target.TakeDamage, damagePack, FinishedAction);
			target = null;
			if(health <= 0) {
				Die();
			}
		}

		DamagePackage damageIntake = null;

		public void TakeDamage(DamagePackage damage) {
			GameController.instance.AddWaitForUnit(this);
			damageIntake = damage;
			if (turnedToStone && DamageConstants.STONEUNITONLYTAKES1DMG) {
				health -= 1;
			}
			else {
				health -= damageIntake.CalculateDamageAgainst(type);
			}
			if (turnedToStone) {
				TookDamage();
			}
			else {
				animHelp.TakeDamage(TookDamage);
			}
			healthIndicator.TookDamage(damageIntake, health);
		}

		void TookDamage() {
			if(health <= 0) {
				//Die unless it can do retaliation.
				if (!damageIntake.WasRetaliation() && DamageConstants.ALLOWRETALIATIONAFTERDEATH && !turnedToStone) {
					RetaliationAttack(damageIntake.GetSource());
					return;
				}
				else {
					Die();
					return;
				}
			}
			//Didn't die, retaliates, if attack was not retaliation (no infinite loops ;) )
			else if(!damageIntake.WasRetaliation() && !turnedToStone) {
				RetaliationAttack(damageIntake.GetSource());
				return;
			}
			damageIntake = null;
			FinishedAction();
		}

		public void Die() {
			GameController.instance.UnitDied(team, currentNode);
			currentNode.RemoveOccupant();
			if (turnedToStone) {
				Death();
			}
			else {
				animHelp.Die(Death);
			}
		}

		void Death() {
			FinishedAction();
			Destroy(gameObject);
		}

		void FinishedAction() {
			GameController.instance.PerformedAction(this);
			FaceForward();
		}

		void FaceForward() {
			transform.LookAt(transform.position + defaultFaceDirection, Vector3.up);
		}

		public void SpawnEntity(Node node, Team team, CombatData data) {
			moveDistance = data.moveDistance;
			damage = data.attackValue;
			type = data.type;
			health = data.healthValue;
			this.team = team;
			currentNode = node;
			stoneUnit = data.stone;
			shadowUnit = data.shadow;
			node.SetOccupant(this);
			this.data = data;
			healthIndicator.Setup(health, damage);
			if (team == Team.Player) {
				defaultFaceDirection = Vector3.right;
				var ma = particles.main;
				ma.startColor = new Color(1, 1, 1, 0.3f);
			}
			else {
				defaultFaceDirection = Vector3.left;
				var ma = particles.main;
				ma.startColor = Color.black;
			}
			FaceForward();
		}

		IEnumerator MoveTo(List<Node> target) {
			animHelp.StartWalk();
			target.Reverse();
			for (int i = 0; i < target.Count; i++) {
				transform.LookAt(target[i].transform);
				bool moving = true;
				while (moving) {
					transform.position += (target[i].transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
					if ((transform.position - target[i].transform.position).magnitude < 0.2f) {
						moving = false;
						
					}
					yield return new WaitForEndOfFrame();
				}
			}
			transform.position = target[target.Count-1].transform.position;
			animHelp.EndWalk();
			FinishedAction();
		}

		public void TurnToStone() {
			if (!stoneUnit) {
				Debug.Log("You cannot turn this unit to stone!");
				return;
			}
			int healthBonus = 0;
			if (DamageConstants.STONEUNITSGETSATTACKASHEALTH) {
				healthBonus = damage;
				health += damage;
			}
			if (DamageConstants.STONEUNITSGETDOUBLEHEALTH) {
				healthBonus = health;
				health *= 2;
			}
			animHelp.TurnToStone();
			damage = 0;
			moveDistance = 0;
			healthIndicator.ChangedAttackValue(damage);
			healthIndicator.GotMoreHealth(health, healthBonus);
			moved = attacked = true;
			turnedToStone = true;
		}

		void OnMouseEnter() {
			TooltipHandler.instance.CreateTooltip(transform.position, this);
		}

		void OnMouseExit() {
			TooltipHandler.instance.CloseTooltip();
		}
	}
}
