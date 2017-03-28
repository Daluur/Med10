using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

namespace CombatWorld.AI {
	public class AISummon {
		private static int summonPoints = AICalculateScore.instance.summonPoints;
		private static readonly int[] unitsToSummon = AICalculateScore.instance.unitsToSummon;
		private static readonly ItemDatabase dataBase = new ItemDatabase();

		private static readonly int triggerForDefensiveSpawns = AICalculateScore.instance.triggerForDefensiveSpawns;

		private static readonly int triggerForOffensiveSpawns = AICalculateScore.instance.triggerForOffensiveSpawns;

		public static void SpawnUnits() {
			summonPoints = AICalculateScore.instance.summonPoints;
			var summonNodes = GameController.instance.GetAISummonNodes();
			var type = EvaluateUnitToSpawn();
			List<SummonNode> toSummonTo = new List<SummonNode>();
			foreach (var node in summonNodes) {
				if (!node.HasOccupant()) {
					toSummonTo.Add(node);
				}
			}
			foreach (var node in toSummonTo) {
				if (node.HasOccupant()) {
					continue;
				}
				if(type.Count>0){
					SpawnUnit(node, unitsToSummon[type[0]]);
					summonPoints -= dataBase.FetchItemByID(unitsToSummon[type[0]]).SummonCost;
					AICalculateScore.instance.SetSummonPoints(summonPoints);
					type.RemoveAt(0);
				}
			}
			type.Clear();
		}

		private static void SpawnUnit(SummonNode node, int type) {
			AICalculateScore.instance.AddAIUnit(SummonHandler.instance.SummonAIUnitByID(node, type));
		}

		private static List<int> EvaluateUnitToSpawn() {
			return BestToSummon(GameController.instance.GetAllUnits());
		}

		private static List<int> BestToSummon(List<Unit> units) {
			var unitToSummon = EnemyUnitAnalysis(units);
			return unitToSummon;
		}

		private static List<int> EnemyUnitAnalysis(List<Unit> units) {

			var highHpFactor = 0;
			var highMoveFactor = 0;
			var highDmgFactor = 0;

			var defensiveFactor = AICalculateScore.instance.defensiveFactor;
			var offensiveFactor = AICalculateScore.instance.offensiveFactor;

			if (defensiveFactor > triggerForDefensiveSpawns && offensiveFactor<defensiveFactor) {
				highHpFactor = 3;
				highDmgFactor = 3;
			}
			else if (offensiveFactor > triggerForOffensiveSpawns && defensiveFactor < offensiveFactor) {
				highMoveFactor = 5;
				highDmgFactor = 2;
			}


			List<CombatData> myUnits = new List<CombatData>();
			for(int i=0;i<unitsToSummon.Length;i++){
				myUnits.Add(new CombatData(dataBase.FetchItemByID(unitsToSummon[i])));
			}

			return BestSummonScore(myUnits, highHpFactor, highMoveFactor, highDmgFactor, RelativeMostOfType());
		}


		private static ElementalTypes RelativeMostOfType(ElementalTypes spawned) {
			int[] amountOfTypes = new int[5];
			ElementalTypes[] mostOfType = new ElementalTypes[5];
			var units = GameController.instance.GetAllUnits();

			foreach (var unit in units) {
				if (unit.GetTeam() != Team.AI) {
					switch (unit.GetElementalType()) {
						case ElementalTypes.NONE:
							mostOfType[0] = ElementalTypes.NONE;
							amountOfTypes[0]++;
							break;
						case ElementalTypes.Nature:
							mostOfType[1] = ElementalTypes.Nature;
							amountOfTypes[1]++;
							break;
						case ElementalTypes.Fire:
							mostOfType[2] = ElementalTypes.Fire;
							amountOfTypes[2]++;
							break;
						case ElementalTypes.Lightning:
							mostOfType[3] = ElementalTypes.Lightning;
							amountOfTypes[3]++;
							break;
						case ElementalTypes.Water:
							mostOfType[4] = ElementalTypes.Water;
							amountOfTypes[4]++;
							break;
						default:
							break;

						}
				}
			}

			for (int i = 0; i < mostOfType.Length; i++) {
				if (mostOfType[i].Equals(spawned)) {
					amountOfTypes[i]--;

				}
			}

			var most = 0;
			var typeWithMost = 0;
			for (int i = 0; i < amountOfTypes.Length; i++) {
				if (amountOfTypes[i] > most) {
					most = amountOfTypes[i];
					typeWithMost = i;
				}
			}
			return mostOfType[typeWithMost];
		}

		private static ElementalTypes RelativeMostOfType() {
			int[] amountOfTypes = new int[5];
			ElementalTypes[] mostOfType = new ElementalTypes[5];
			var units = GameController.instance.GetAllUnits();

			foreach (var unit in units) {
				if (unit.GetTeam() != Team.AI) {
					switch (unit.GetElementalType()) {
						case ElementalTypes.NONE:
							mostOfType[0] = ElementalTypes.NONE;
							amountOfTypes[0]++;
							break;
						case ElementalTypes.Nature:
							mostOfType[1] = ElementalTypes.Nature;
							amountOfTypes[1]++;
							break;
						case ElementalTypes.Fire:
							mostOfType[2] = ElementalTypes.Fire;
							amountOfTypes[2]++;
							break;
						case ElementalTypes.Lightning:
							mostOfType[3] = ElementalTypes.Lightning;
							amountOfTypes[3]++;
							break;
						case ElementalTypes.Water:
							mostOfType[4] = ElementalTypes.Water;
							amountOfTypes[4]++;
							break;
						default:
							break;
					}
				}
			}
			var most = 0;
			var typeWithMost = 0;
			for (int i = 0; i < amountOfTypes.Length; i++) {
				if (amountOfTypes[i] > most) {
					most = amountOfTypes[i];
					typeWithMost = i;
				}
			}
			return mostOfType[typeWithMost];
		}

		private static List<int> BestSummonScore(List<CombatData> units, int highHPFactor, int highMoveFactor, int highDMGFactor, ElementalTypes mostOfType) {
			List<int> index = new List<int>();
			int ind = -1;
			int score = 0;
			int bestScore = -1;
			int calculatedSummonPoints = summonPoints;
			int iterator = 0;
			int maxIterations = 10;
			while (calculatedSummonPoints > 0) {
				if (iterator >= maxIterations) {
					break;
				}
				for(int i=0; i<units.Count;i++) {
					if (units[i].cost <= calculatedSummonPoints && calculatedSummonPoints - units[i].cost >= 0) {
						score = Mathf.RoundToInt((highHPFactor * units[i].healthValue + highMoveFactor * units[i].moveDistance + highDMGFactor * units[i].attackValue)*AIUtilityMethods.TypeModifier(units[i].type,mostOfType));
						if (score > bestScore && score > 1) {
							bestScore = score;
							ind = i;
						}
					}
				}
				if(ind!=-1&&calculatedSummonPoints-units[ind].cost>=0){
					calculatedSummonPoints -= units[ind].cost;
					Debug.Log("Wanting to summon: " + units[ind].model.name + " for: " + units[ind].cost + " " + calculatedSummonPoints);
					index.Add(ind);
					mostOfType = RelativeMostOfType(units[ind].type);
					Debug.Log(mostOfType);
				}
				iterator++;
			}
			return index;
		}

	}
}