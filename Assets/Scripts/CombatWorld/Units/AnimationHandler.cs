using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class AnimationHandler : MonoBehaviour {

		string attackName = "Melee Right Attack 01";
		bool playingAnimation;
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
				if(attackName == "Left Punch Attack" || attackName == "Right Punch Attack") {
					attackName = "Melee Right Attack 01";
				}
				attackName = "Fly " + attackName;
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

		public void Step() {
			AudioHandler.instance.PlayMove();
		}

		public void Attack(Action cb) {
			AudioHandler.instance.PlayAttack();
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

		public void TakeDamageSound() {
			AudioHandler.instance.PlayTakeDamage();
		}

		public void Die(Action cb) {
			nextCB.Enqueue(cb);
			QueueAnim(dieTrigger);
		}

		public void FinishedDie() {
			FinishedAnim();
		}

		public void DieSound() {
			AudioHandler.instance.PlayDie();
		}

		public void TurnToStone(Action cb) {
			StartCoroutine(TurnToStoneColor(cb));
			anim.SetBool("Defend", true);
		}

		public void FinishedDefend(Action cb) {
			anim.Stop();
			cb();
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

		IEnumerator TurnToStoneColor(Action cb) {
			var renderers = GetComponentsInChildren<Renderer>();
			Material[] materials = new Material[renderers.Length];
			for (int i = 0; i < renderers.Length; i++) {
				materials[i] = renderers[i].material;
			}
			Material end = Resources.Load<Material>("Art/Materials/Grey");
			float t = 0;
			Material mat;
			while (t < 1) {
				for (int i = 0; i < renderers.Length; i++) {
					mat = renderers[i].material;
					mat.Lerp(materials[i], end, t);
					renderers[i].material = mat;
				}
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			FinishedDefend(cb);
		}
	}
}