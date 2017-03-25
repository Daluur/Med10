using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Units;

namespace CombatWorld.Units {
	public class HealthAttackVisualController : MonoBehaviour {

		public Text health;
		public Text attack;
		public Text CombatText;
		public Animator anim;

		public void Setup(int hp, int attackval) {
			health.text = hp.ToString();
			attack.text = attackval.ToString();
		}

		public void TookDamage(DamagePackage dmg, int newHealth) {
			health.text = newHealth.ToString();
			CombatText.text = "-"+dmg.GetCalculatedDMG();
			anim.SetTrigger("TakeDMG");
		}

		public void GotMoreHealth(int newHealth, int bonus) {
			health.text = newHealth.ToString();
		}

		public void ChangedAttackValue(int newVal) {
			attack.text = newVal.ToString();
		}
	}
}