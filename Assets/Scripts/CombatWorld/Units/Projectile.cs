using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class Projectile : MonoBehaviour {

		Transform target;
		float speed = 25;
		Action<DamagePackage> CB;
		Action finishCB;
		Vector3 dir;
		DamagePackage damage;

		public void Setup(Transform target, Action<DamagePackage> cb, DamagePackage damage, Action finishCB) {
			this.target = target;
			this.damage = damage;
			transform.LookAt(target);
			dir = target.transform.position - transform.position;
			dir.Normalize();
			CB = cb;
			this.finishCB = finishCB;
			StartCoroutine(Travel());
		}

		IEnumerator Travel() {
			bool hit = false;
			while (!hit) {
				transform.position += dir * speed * Time.deltaTime;
				if((transform.position - target.transform.position).magnitude < 1) {
					hit = true;
				}
				yield return new WaitForEndOfFrame();
			}
			CB(damage);
			finishCB();
			Destroy(gameObject);
		}
	}
}