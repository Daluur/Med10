using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;
using UnityEngine.EventSystems;
using Overworld;

namespace CombatWorld.Units {
	public class Unit : MonoBehaviour, IEntity {
		[Tooltip("Name of the trigger. Useable: \nMelee Right Attack 01 \nMelee Right Attack 02 \nMelee Right Attack 03 \nMelee Left Attack 01 \nLeft Punch Attack \nRight Punch Attack \nProjectile Right Attack 01 \nCrossbow Attack \nCast Spell 01 \nCast Spell 02")]
		public string attackName = "Melee Right Attack 01";
		public GameObject projectile;

		private bool waitForProjectile = false;

		private bool shadowUnit = false;
		private bool stoneUnit = false;
		public bool turnedToStone = false;

		private int health;
		private int maxHealth;
		private Team team;
		private Node currentNode;

		private ElementalTypes type;

		private int moveDistance;

		private int damage;

		private bool moved = true;
		private bool attacked = true;

		public bool doingAction = false;

		private Vector3 defaultFaceDirection;

		public CombatData data;

		private float moveSpeed = 7f;

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

		public void Move(List<Node> node, bool AIAttackAfter) {
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
			movedThroughUnitThisTurn = false;
			moved = attacked = false;
		}

		IEntity target;
		DamagePackage damagePack;

		public void Attack(IEntity target) {
			GameController.instance.AddWaitForUnit(this);
			this.target = target;
			attacked = moved = true;
			damagePack = new DamagePackage(damage, this, type);
			damagePack.info.stone = turnedToStone;
			damagePack.info.shadow = shadowUnit;
			damagePack.info.movedThroughUnit = movedThroughUnitThisTurn;
			DealDamage();
			if (DamageConstants.ATTACKATSAMETIME) {
				if (target.GetNode().HasUnit()) {
					target.GetNode().GetUnit().RetaliationAttack(this);
				}
			}
		}

		public void RetaliationAttack(IEntity target) {
			this.target = target;
			damagePack = new DamagePackage(damage, this, type, true);
			damagePack.info.stone = turnedToStone;
			damagePack.info.shadow = shadowUnit;
			DealDamage();
		}

		void DealDamage() {
			transform.LookAt(target.GetTransform());
			animHelp.Attack(SpawnProjectile);
		}

		void SpawnProjectile() {
			waitForProjectile = true;
			if(health <= 0) {
				waitForDeathAnim = true;
			}
			Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<BasicProjectile>().Setup(target.GetTransform(), target.TakeDamage, damagePack, ProjectileHit);
			if (!DamageConstants.ATTACKATSAMETIME) {
				if(health <= 0) {
					Die();
				}
			}
		}

		DamagePackage damageIntake = null;

		public void TakeDamage(DamagePackage damage) {
			GameController.instance.AddWaitForUnit(this);
			damageIntake = damage;
			if (turnedToStone && StoneUnitOptions.STONEUNITTAKESSTATICDMG) {
				health -= damageIntake.TargetWasStone();
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
			healthIndicator.TookDamage(damageIntake, (float)health/maxHealth);
		}

		void TookDamage() {
			if (DamageConstants.ATTACKATSAMETIME) {
				if (health <= 0) {
					damageIntake.info.killHit = true;
					Die();
					return;
				}
			}
			else {
				if (health <= 0) {
					//Die unless it can do retaliation.
					if (!damageIntake.WasRetaliation() && DamageConstants.ALLOWRETALIATIONAFTERDEATH && (!turnedToStone || StoneUnitOptions.STONEUNITCANRETALIATE)) {
						RetaliationAttack(damageIntake.GetSource());
						return;
					}
					else {
						damageIntake.info.killHit = true;
						Die();
						return;
					}
					damageIntake.info.killHit = true;
					Die();
					return;
				}
				//Didn't die, retaliates, if attack was not retaliation (no infinite loops ;) )
				else if(!damageIntake.WasRetaliation() && (!turnedToStone || StoneUnitOptions.STONEUNITCANRETALIATE)) {
					RetaliationAttack(damageIntake.GetSource());
					return;
				}
			}
			if (damageIntake.WasRetaliation()) {
				FaceForward();
			}
			damageIntake = null;
			FinishedAction();
		}

		public void Die() {
			GameController.instance.UnitDied(team, currentNode);
			currentNode.RemoveOccupant();
			waitForDeathAnim = true;
			if (team == Team.AI) {
				healthIndicator.SummonPoint(false);
			}
			if (turnedToStone) {
				Death();
			}
			else {
				animHelp.Die(Death);
			}
		}

		void Death() {
			waitForDeathAnim = false;
			RealDeath();
		}

		bool waitForDeathAnim = false;

		void RealDeath() {
			if(waitForProjectile || waitForDeathAnim) {
				return;
			}
			FinishedAction();
			Destroy(gameObject);
		}

		void FinishedAction() {
			GameController.instance.PerformedAction(this);
		}

		void ProjectileHit() {
			waitForProjectile = false;
			if (!DamageConstants.ATTACKATSAMETIME) {
				if (health <= 0) {
					RealDeath();
				}
				else {
					FinishedAction();
				}
			}
			else {
				FinishedAction();
			}
			if ((damagePack.WasRetaliation() && health > 0) || target.GetNode().HasTower()) {
				FaceForward();
			}
		}

		void FaceForward() {
			transform.LookAt(transform.position + defaultFaceDirection, Vector3.up);
		}

		public void SpawnEntity(Node node, Team team, CombatData data) {
			if(team == Team.Player) {
				DataGathering.Instance.SummonedNewUnit(new SummonPlayerData() {type = data.type, shadow = data.shadow, stone = data.stone });
			}
			moveDistance = data.moveDistance;
			damage = data.attackValue;
			type = data.type;
			maxHealth = health = data.healthValue;
			this.team = team;
			currentNode = node;
			stoneUnit = data.stone;
			shadowUnit = data.shadow;
			node.SetOccupant(this);
			this.data = data;
			if (healthIndicator == null)
				healthIndicator = GetComponentInChildren<HealthAttackVisualController>();
			healthIndicator.Setup(health, damage, type, shadowUnit, stoneUnit);
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

		bool movedThroughUnitThisTurn = false;

		IEnumerator MoveTo(List<Node> target) {
			CombatCameraController.instance.SetTarget(transform);
			if(target.Count == 1) {
				FinishedAction();
				yield break;
			}
			animHelp.StartWalk();
			target.Reverse();
			for (int i = 1; i < target.Count; i++) {
				if (shadowUnit) {
					if (target[i].HasOccupant()) {
						movedThroughUnitThisTurn = true;
					}
				}
				transform.LookAt(target[i].transform);
				//moveDir = (target[i].transform.position - transform.position).normalized;
				bool moving = true;
				while (moving) {
					transform.position += (target[i].transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
					if ((transform.position - target[i].transform.position).magnitude < 0.5f) {
						moving = false;
					}
					/*if((target[i].transform.position - transform.position).normalized != moveDir) {
						moving = false;
					}*/
					yield return new WaitForEndOfFrame();
				}
			}
			if(shadowUnit && !movedThroughUnitThisTurn) {
				DataGathering.Instance.MoveShadowNotThroughUnits();
			}
			else if(shadowUnit && movedThroughUnitThisTurn) {
				DataGathering.Instance.MovedShadowThroughUnit();
			}
			transform.position = target[target.Count-1].transform.position;
			animHelp.EndWalk();
			/*if (AIAttackAfter) {
				FinishedImediateAction();
				yield break;
			}*/
			FinishedAction();
			FaceForward();
		}

#pragma warning disable 0162
		public void TurnToStone() {
			if (!stoneUnit) {
				Debug.Log("You cannot turn this unit to stone!");
				return;
			}
			if(team == Team.Player) {
				DataGathering.Instance.TurnedUnitToStone();
			}
			int healthBonus = 0;
			if (StoneUnitOptions.STONEUNITSGETSATTACKASHEALTH) {
				healthBonus = damage;
				health += damage;
			}
			if (StoneUnitOptions.STONEUNITSGETDOUBLEHEALTH) {
				healthBonus = health;
				health *= 2;
			}
			GameController.instance.AddWaitForUnit(this);
			
			if (!StoneUnitOptions.STONEUNITCANRETALIATE) {
				animHelp.TurnToStone(TurnedToStone);
				damage = 0;
			}
			else {
				FinishedAction();
				damage = StoneUnitOptions.STONEUNITRETALIATEDMG;
			}
			
			moveDistance = 0;
			healthIndicator.ChangedAttackValue(damage);
			healthIndicator.GotMoreHealth(health, healthBonus);
			moved = attacked = true;
			turnedToStone = true;
		}
#pragma warning restore 0162

		public bool GetShadow() {
			return shadowUnit;
		}

		void TurnedToStone() {
			FinishedAction();
		}

		void OnMouseOver() {
			TooltipHandler.instance.CreateTooltip(transform.position, this);
			if(CursorSingleton.instance!=null)
				CursorSingleton.instance.SetCursor(currentNode.GetState());
		}

		void OnMouseExit() {
			TooltipHandler.instance.CloseTooltip();
			if(CursorSingleton.instance!=null)
				CursorSingleton.instance.SetCursor();
		}

		void OnDestroy() {
			TooltipHandler.instance.CloseTooltip();
		}

		private void OnMouseDown() {
			currentNode.HandleInput();
		}
	}
}
