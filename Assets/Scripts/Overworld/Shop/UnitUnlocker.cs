using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overworld.Shops {
	public class UnitUnlocker : MonoBehaviour {

		public int IDToUnlock;

		private void OnTriggerEnter(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			UnlockHandler.Instance.UnlockUnitByID(IDToUnlock);
			Destroy(gameObject);
		}
	}
}