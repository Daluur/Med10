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
				UnlockedUnits.Add(0);
				UnlockedUnits.Add(12);
				UnlockedUnits.Add(13);
				UnlockedUnits.Add(1);
			}
		}

		public void UnlockUnitByID(int id) {
			if (UnlockedUnits.Contains(id)) {
				Debug.LogError("ID: "+id+" Was already unlocked!");
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