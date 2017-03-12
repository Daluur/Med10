using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class AnimationHelper : MonoBehaviour {

		Animator anim;

		bool animationIsRunning = false;

		Queue<string> nextAnim = new Queue<string>();
		Queue<Action> nextCB = new Queue<Action>();

		// Use this for initialization
		void Start() {
			anim = GetComponentInChildren<Animator>();
		}

		public void Attack(Transform target, Action cb) {
			transform.LookAt(target,Vector3.up);
			nextAnim.Enqueue("Melee Right Attack 01");
			nextCB.Enqueue(cb);
			if (!animationIsRunning) {
				StartCoroutine(PlayAnimations());
			}
		}

		public void TakeDamage(Action cb) {
			nextAnim.Enqueue("Take Damage");
			nextCB.Enqueue(cb);
			if (!animationIsRunning) {
				StartCoroutine(PlayAnimations());
			}
		}

		public void StartWalk() {
			anim.SetBool("Run", true);
		}

		public void EndWalk() {
			anim.SetBool("Run", false);
		}

		public void Die(Action cb) {
			nextAnim.Enqueue("Die");
			nextCB.Enqueue(cb);
			if (!animationIsRunning) {
				StartCoroutine(PlayAnimations());
			}
		}

		IEnumerator PlayAnimations() {
			animationIsRunning = true;
			while(nextAnim.Count > 0) {
				anim.SetTrigger(nextAnim.Dequeue());
				yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0));
				nextCB.Dequeue()();
			}
			animationIsRunning = false;
		}
	}
}