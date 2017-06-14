using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld.Shops {
	public class UnlockHandler {

		#region Singleton Things

		private static UnlockHandler instance;

		public static UnlockHandler Instance {
			get {
				if (instance == null) {
					instance = new UnlockHandler();
				}
				return instance;
			}
		}

		#endregion

		List<int> UnlockedUnits = new List<int>();
		bool newUnlock = true;

		private UnlockHandler() {
			List<int> savedUnlocks = SaveLoadHandler.Instance.GetUnlockedUnits();
			if (savedUnlocks.Count != 0) {
				foreach (int item in savedUnlocks) {
					UnlockedUnits.Add(item);
				}
			}
			else {
				/*for (int i = 0; i < 14; i++) {
					UnlockedUnits.Add (i);
				}*/
				UnlockedUnits.Add(0);
				UnlockedUnits.Add (1);
				UnlockedUnits.Add (2);
				UnlockedUnits.Add (3);
				UnlockedUnits.Add (4);
				UnlockedUnits.Add (5);
				UnlockedUnits.Add (6);
				UnlockedUnits.Add (7);
				UnlockedUnits.Add (8);
				UnlockedUnits.Add (9);
				UnlockedUnits.Add (10);
				UnlockedUnits.Add (11);
				UnlockedUnits.Add (12);
			}
		}

		public void UnlockUnitByID(int id) {
			if (UnlockedUnits.Contains(id)) {
				//Debug.LogError("ID: "+id+" Was already unlocked!");
			}
			else {
				UnlockedUnits.Add(id);
				newUnlock = true;
			}
		}

		public List<int> GetUnlockedUnits() {
			return UnlockedUnits;
		}

		public int UnlockedCount() {
			return UnlockedUnits.Count;
		}

		public bool UnlockedNewUnitSinceLast() {
			if (newUnlock) {
				newUnlock = false;
				return true;
			}
			return false;
		}
	}
}