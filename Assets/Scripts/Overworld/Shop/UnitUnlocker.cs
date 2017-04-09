using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overworld.Shops {
	public class UnitUnlocker : MonoBehaviour {

		public int[] IDToUnlock;

		private void OnTriggerEnter(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			for (int i = 0; i < IDToUnlock.Length; i++) {
				UnlockHandler.Instance.UnlockUnitByID(IDToUnlock[i]);
			}
			Destroy(gameObject);
		}
	}
}