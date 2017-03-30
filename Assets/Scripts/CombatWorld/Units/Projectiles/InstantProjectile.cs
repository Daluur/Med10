using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatWorld.Units {
	public class InstantProjectile : BasicProjectile {

		protected override IEnumerator Travel() {
			Instantiate(HitEffect, target.position, Quaternion.identity);
			Hit();
			yield break;
		}
	}
}