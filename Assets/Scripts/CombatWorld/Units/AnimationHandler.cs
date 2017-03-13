using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class AnimationHandler : MonoBehaviour {

		string attackName = "Melee Right Attack 01";
		bool playingAnimation;

		Animator anim;

		Queue<Action> nextCB = new Queue<Action>();
		Queue<string> nextAnim = new Queue<string>();

		// Use this for initialization
		void Start() {
			anim = GetComponent<Animator>();
		}

		public AnimationHandler Setup(string attackName) {
			this.attackName = attackName;
			return this;
		}

		public void Attack(Transform target, Action cb) {
			transform.LookAt(target, Vector3.up);
			nextCB.Enqueue(cb);
			QueueAnim(attackName);
		}

		public void FinishedAttack() {
			FinishedAnim();
		}

		public void TakeDamage(Action cb) {
			nextCB.Enqueue(cb);
			QueueAnim("Take Damage");
		}

		public void FinishedTakeDamage() {
			FinishedAnim();
		}

		public void Die(Action cb) {
			nextCB.Enqueue(cb);
			QueueAnim("Die");
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