using System.Collections;
using System.Collections.Generic;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class DamagePackage {

		int damage;
		ElementalTypes damageType = ElementalTypes.NONE;
		Unit source;
		bool wasRetaliation = false;

		public DamagePackage(int damage, Unit source, ElementalTypes type = ElementalTypes.NONE, bool retaliation = false) {
			this.damage = damage;
			this.source = source;
			damageType = type;
			wasRetaliation = retaliation;
		}
		
		public Unit GetSource() {
			return source;
		}

		public bool WasRetaliation() {
			return wasRetaliation;
		}

		public int CalculateDamageAgainst(ElementalTypes type) {
			switch (type) {
				case ElementalTypes.NONE:
					break;
				case ElementalTypes.Fire:
					if(damageType == ElementalTypes.Water) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if(damageType == ElementalTypes.Earth) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Water:
					if (damageType == ElementalTypes.Ligthning) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Fire) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Earth:
					if (damageType == ElementalTypes.Fire) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Ligthning) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Ligthning:
					if (damageType == ElementalTypes.Earth) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Water) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				default:
					break;
			}
			return damage;
		}
	}
}
