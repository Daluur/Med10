using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class BasicProjectile: MonoBehaviour {

		public GameObject HitEffect;

		protected Transform target;
		public float speed = 25;
		Action<DamagePackage> CB;
		Action finishCB;
		protected  Vector3 dir;
		DamagePackage damage;

		public void Setup(Transform target, Action<DamagePackage> cb, DamagePackage damage, Action finishCB) {
			this.target = target;
			this.damage = damage;
			CB = cb;
			this.finishCB = finishCB;
			StartCoroutine(Travel());
		}

		protected virtual IEnumerator Travel() {
			transform.LookAt(target);
			dir = target.transform.position - transform.position;
			dir.Normalize();
			bool hit = false;
			while (!hit) {
				transform.position += dir * speed * Time.deltaTime;
				if((transform.position - target.transform.position).magnitude < 1) {
					hit = true;
				}
				yield return new WaitForEndOfFrame();
			}
			if(HitEffect != null) {
				Instantiate(HitEffect, transform.position, Quaternion.identity);
			}
			Hit();
		}

		protected void Hit() {
			CB(damage);
			finishCB();
			Destroy(gameObject);
		}
	}
}