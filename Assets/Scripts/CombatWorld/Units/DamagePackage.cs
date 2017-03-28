using System.Collections;
using System.Collections.Generic;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class DamagePackage {

		int damage;
		int damageDone;
		ElementalTypes damageType = ElementalTypes.NONE;
		IEntity source;
		bool wasRetaliation = false;

		public DamagePackage(int damage, IEntity source, ElementalTypes type = ElementalTypes.NONE, bool retaliation = false) {
			this.damage = damage;
			this.source = source;
			damageType = type;
			wasRetaliation = retaliation;
		}
		
		public IEntity GetSource() {
			return source;
		}

		public bool WasRetaliation() {
			return wasRetaliation;
		}

		public int CalculateDamageAgainst(ElementalTypes type = ElementalTypes.NONE) {
			damageDone = damage;
			switch (type) {
				case ElementalTypes.Fire:
					if(damageType == ElementalTypes.Water) {
						damageDone = damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if(damageType == ElementalTypes.Nature) {
						damageDone = (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Water:
					if (damageType == ElementalTypes.Lightning) {
						damageDone = damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Fire) {
						damageDone = (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Nature:
					if (damageType == ElementalTypes.Fire) {
						damageDone = damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Lightning) {
						damageDone = (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Lightning:
					if (damageType == ElementalTypes.Nature) {
						damageDone = damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Water) {
						damageDone = (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.NONE:
				default:
					break;
			}
			return damageDone;
		}

		/// <summary>
		/// Should only be used for combatText.
		/// </summary>
		public int GetCalculatedDMG() {
			return damageDone;
		}
	}
}
