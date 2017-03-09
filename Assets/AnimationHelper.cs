using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class AnimationHelper : MonoBehaviour {

		Animator anim;

		bool animationIsRunning = false;

		// Use this for initialization
		void Start() {
			anim = GetComponentInChildren<Animator>();
		}

		public void Attack(Transform target, Action<DamagePackage, Entity> cb, DamagePackage DP, Entity entity) {
			Debug.Log("Start attack " + Time.timeSinceLevelLoad);
			transform.LookAt(target,Vector3.up);
			StartCoroutine(PlayAnimation("Melee Right Attack 01", cb, DP, entity));
		}

		public void TakeDamage(Action<DamagePackage> cb, DamagePackage DP) {
			Debug.Log("Starts take damage " + Time.timeSinceLevelLoad);
			StartCoroutine(PlayAnimation("Take Damage", cb, DP));
		}

		public void StartWalk() {
			anim.SetBool("Run", true);
		}

		public void EndWalk() {
			anim.SetBool("Run", false);
		}

		public void Die(Action cb) {
			Debug.Log("Starts death "+Time.timeSinceLevelLoad);
			StartCoroutine(PlayAnimation("Die", cb));
		}

		IEnumerator PlayAnimation(string animationName, Action cb) {
			yield return new WaitUntil(() => animationIsRunning == false);
			animationIsRunning = true;
			anim.SetTrigger(animationName);

			yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0));
			animationIsRunning = false;
			Debug.Log("death finished, does callback " + Time.timeSinceLevelLoad);
			cb();
		}

		IEnumerator PlayAnimation(string animationName, Action<DamagePackage, Entity> cb, DamagePackage DP, Entity entity) {
			yield return new WaitUntil(() => animationIsRunning == false);
			animationIsRunning = true;
			anim.SetTrigger(animationName);
			
			yield return new WaitUntil(()=> anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0));
			animationIsRunning = false;
			Debug.Log("Attack finished, does callback " + Time.timeSinceLevelLoad);
			cb(DP, entity);
		}

		IEnumerator PlayAnimation(string animationName, Action<DamagePackage> cb, DamagePackage DP) {
			yield return new WaitUntil(() => animationIsRunning == false);
			animationIsRunning = true;
			anim.SetTrigger(animationName);

			yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0));
			animationIsRunning = false;
			Debug.Log("took damage finished, does callback " + Time.timeSinceLevelLoad);
			cb(DP);
		}
	}
}