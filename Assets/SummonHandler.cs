using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class SummonHandler : Singleton<SummonHandler>
	{
		public GameObject UnitToSummon;

		public void SummonUnit(SummonNode node)
		{
			GameObject unit = Instantiate(UnitToSummon, node.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
			unit.GetComponent<Unit>().SpawnEntity(node);
		}

		public void SummonButtonPressed()
		{
			GameController.instance.HighlightSummonNodes();
		}
	}
}