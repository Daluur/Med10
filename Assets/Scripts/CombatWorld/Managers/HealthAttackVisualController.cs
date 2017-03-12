using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Units;

namespace CombatWorld {
	public class HealthAttackVisualController : MonoBehaviour {

		public Text health;
		public Text attack;
		public Entity source;

		private void Start() {
			source = GetComponentInParent<Entity>();
		}

		// Update is called once per frame
		void Update() {
			health.text = source.GetHealth().ToString();
			if (source.GetType() == typeof(Unit)) {
				var temp = (Unit)source;
				attack.text = temp.GetAttackValue().ToString();
			}
			else {
				attack.text = "0";
			}
		}
	}
}