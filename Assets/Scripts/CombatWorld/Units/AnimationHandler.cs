using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class AnimationHandler : MonoBehaviour {

		string attackName = "Melee Right Attack 01";
		bool playingAnimation;
		bool shadowUnit = false;
		string walkTrigger = "Run";
		string takeDamageTrigger = "Take Damage";
		string dieTrigger = "Die";

		Animator anim;

		Queue<Action> nextCB = new Queue<Action>();
		Queue<string> nextAnim = new Queue<string>();

		// Use this for initialization
		void Awake() {
			anim = GetComponent<Animator>();
		}

		public AnimationHandler Setup(string attackName, bool shadowUnit) {
			if (shadowUnit) {
				anim.SetBool("Fly Idle", true);
				walkTrigger = "Fly Forward";
				takeDamageTrigger = "Fly Take Damage";
				dieTrigger = "Fly Die";
			}
			this.attackName = attackName;
			return this;
		}

		public void StartWalk() {
			anim.SetBool(walkTrigger,true);
		}

		public void EndWalk() {
			anim.SetBool(walkTrigger, false);
		}

		public void Attack(Action cb) {
			nextCB.Enqueue(cb);
			QueueAnim(attackName);
		}

		public void FinishedAttack() {
			FinishedAnim();
		}

		public void TakeDamage(Action cb) {
			nextCB.Enqueue(cb);
			QueueAnim(takeDamageTrigger);
		}

		public void FinishedTakeDamage() {
			FinishedAnim();
		}

		public void Die(Action cb) {
			nextCB.Enqueue(cb);
			QueueAnim(dieTrigger);
		}

		public void FinishedDie() {
			FinishedAnim();
		}

		void QueueAnim(String name) {
			if (playingAnimation) {
				nextAnim.Enqueue(name);
			}
			else {
				playingAnimation = true;
				anim.SetTrigger(name);
			}
		}

		void FinishedAnim() {
			if (nextAnim.Count > 0) {
				anim.SetTrigger(nextAnim.Dequeue());
			}
			else {
				playingAnimation = false;
			}
			nextCB.Dequeue()();
		}
	}
}