using System.Collections;
using System.Collections.Generic;
using CombatWorld.Utility;

namespace CombatWorld.Units {
	public class DamagePackage {

		int damage;
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
			switch (type) {
				case ElementalTypes.NONE:
					break;
				case ElementalTypes.Fire:
					if(damageType == ElementalTypes.Water) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if(damageType == ElementalTypes.Nature) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Water:
					if (damageType == ElementalTypes.Lightning) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Fire) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Nature:
					if (damageType == ElementalTypes.Fire) {
						return damage * DamageConstants.EFFECTIVEMULTIPLIER;
					}
					else if (damageType == ElementalTypes.Lightning) {
						return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
					}
					break;
				case ElementalTypes.Lightning:
					if (damageType == ElementalTypes.Nature) {
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
