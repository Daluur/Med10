using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using CombatWorld.Map;

namespace CombatWorld.Units {
	public interface IEntity {

		Transform GetTransform();
		Team GetTeam();
		Node GetNode();
		int GetHealth();
		void TakeDamage(DamagePackage damage);
		void Die();

	}
}
