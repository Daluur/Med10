using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class ProjectileFromSky : BasicProjectile {

		float distAbove = 20;

		protected override IEnumerator Travel() {
			transform.position = target.position + new Vector3(0, distAbove, 0);
			transform.LookAt(target);
			dir = target.transform.position - transform.position;
			dir.Normalize();
			bool hit = false;
			while (!hit) {
				transform.position += dir * speed * Time.deltaTime;
				if ((transform.position - target.transform.position).magnitude < 1) {
					hit = true;
				}
				yield return new WaitForEndOfFrame();
			}
			if (HitEffect != null) {
				Instantiate(HitEffect, transform.position, Quaternion.identity);
			}
			Hit();
		}
	}
}
