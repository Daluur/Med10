using System.Collections;
using System.Collections.Generic;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class DamagePackage {

		int damage;
		ElementalTypes damageType;

		public DamagePackage(int damage) {
			this.damage = damage;
			damageType = ElementalTypes.NONE;
		}

		public DamagePackage(int damage, ElementalTypes type) {
			this.damage = damage;
			damageType = type;
		}

		public int CalculateDamageAgainst(ElementalTypes type) {
			switch (type) {
				case ElementalTypes.NONE:
					break;
				case ElementalTypes.Fire:
					if(damageType == ElementalTypes.Water) {
						return damage * DamageMultipliers.EFFECTIVEMULTIPLIER;
					}
					else if(damageType == ElementalTypes.Earth) {
						return (int)(damage * DamageMultipliers.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Water:
					if (damageType == ElementalTypes.Ligthning) {
						return damage * DamageMultipliers.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Fire) {
						return (int)(damage * DamageMultipliers.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Earth:
					if (damageType == ElementalTypes.Fire) {
						return damage * DamageMultipliers.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Ligthning) {
						return (int)(damage * DamageMultipliers.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Ligthning:
					if (damageType == ElementalTypes.Earth) {
						return damage * DamageMultipliers.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Water) {
						return (int)(damage * DamageMultipliers.INEFFECTIVEMULTIPLIER);
					}
					break;
				default:
					break;
			}
			return damage;
		}
	}
}
