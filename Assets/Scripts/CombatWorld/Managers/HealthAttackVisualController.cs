using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class HealthAttackVisualController : MonoBehaviour {

		public Text health;
		public Text attack;
		public Text CombatText;
		public Image icon;
		public Animator anim;

		public void Setup(int hp, int attackval, ElementalTypes type = ElementalTypes.NONE, bool shadow = false, bool stone = false) {
			health.text = hp.ToString();
			attack.text = attackval.ToString();
			if(shadow) {
				ApplyShadowIcon();
			}
			else if (stone) {
				ApplyStoneIcon();
			}
			else {
				ApplyIconByType(type);
			}
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

		void ApplyIconByType(ElementalTypes type) {
			switch (type) {
				case ElementalTypes.Fire:
					icon.sprite = Resources.Load<Sprite>("Art/2D/Icons/Fire");
					break;
				case ElementalTypes.Water:
					icon.sprite = Resources.Load<Sprite>("Art/2D/Icons/Water");
					break;
				case ElementalTypes.Nature:
					icon.sprite = Resources.Load<Sprite>("Art/2D/Icons/Nature");
					break;
				case ElementalTypes.Lightning:
					icon.sprite = Resources.Load<Sprite>("Art/2D/Icons/Lightning");
					break;
				case ElementalTypes.NONE:
				default:
					icon.enabled = false;
					break;
			}
		}

		void ApplyShadowIcon() {
			icon.sprite = Resources.Load<Sprite>("Art/2D/Icons/Shadow");
		}

		void ApplyStoneIcon() {
			icon.sprite = Resources.Load<Sprite>("Art/2D/Icons/Stone");
		}
	}
}