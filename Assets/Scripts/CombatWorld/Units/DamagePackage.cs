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
						damageDone = GetEffectiveDamage(damage);
					}
					else if(damageType == ElementalTypes.Nature) {
						damageDone = GetInEffectivDamage(damage);
					}
					break;
				case ElementalTypes.Water:
					if (damageType == ElementalTypes.Lightning) {
						damageDone = GetEffectiveDamage(damage);
					}
					else if (damageType == ElementalTypes.Fire) {
						damageDone = GetInEffectivDamage(damage);
					}
					break;
				case ElementalTypes.Nature:
					if (damageType == ElementalTypes.Fire) {
						damageDone = GetEffectiveDamage(damage);
					}
					else if (damageType == ElementalTypes.Lightning) {
						damageDone = GetInEffectivDamage(damage);
					}
					break;
				case ElementalTypes.Lightning:
					if (damageType == ElementalTypes.Nature) {
						damageDone = GetEffectiveDamage(damage);
					}
					else if (damageType == ElementalTypes.Water) {
						damageDone = GetInEffectivDamage(damage);
					}
					break;
				case ElementalTypes.NONE:
				default:
					break;
			}
			return damageDone;
		}

#pragma warning disable 0162
		int GetEffectiveDamage(int damage) {
			if(source.GetTeam() == Team.Player) {
				switch (damageType) {
					case ElementalTypes.Fire:
						DataCollection.instance.PerformedAction(ActionType.FireTrade);
						break;
					case ElementalTypes.Water:
						DataCollection.instance.PerformedAction(ActionType.WaterTrade);
						break;
					case ElementalTypes.Nature:
						DataCollection.instance.PerformedAction(ActionType.NatureTrade);
						break;
					case ElementalTypes.Lightning:
						DataCollection.instance.PerformedAction(ActionType.LightningTrade);
						break;
					default:
						break;
				}
			}
			if (DamageConstants.EFFECTIVEMULT) {
				return damage * DamageConstants.EFFECTIVEMULTIPLIER;
			}
			else {
				return damage + DamageConstants.EFFECTIVEBONUS;
			}
		}

		int GetInEffectivDamage(int damage) {
			if (DamageConstants.EFFECTIVEMULT) {
				return (int)(damage * DamageConstants.INEFFECTIVEMULTIPLIER);
			}
			else {
				return damage - DamageConstants.INEFFECTIVEPENALTY;
			}
		}
#pragma warning restore 0162

		/// <summary>
		/// Should only be used for combatText.
		/// </summary>
		public int GetCalculatedDMG() {
			return damageDone;
		}
	}
}
