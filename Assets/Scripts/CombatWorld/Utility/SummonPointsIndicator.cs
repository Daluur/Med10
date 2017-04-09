using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CombatWorld {
	public class SummonPointsIndicator : Singleton<SummonPointsIndicator> {
		public GameObject summonPointsIndicatorGO;
		private Text text;
		public float floatingSpeed = 3f;
		public float displacement = 20f;
		public List<GameObject> gos = new List<GameObject>();
		private void Start() {
			//text = summonPointsIndicatorGO.GetComponentInChildren<Text>();
		}

		public void ShowSummonPoints(GameObject go) {


		}
	}
}
