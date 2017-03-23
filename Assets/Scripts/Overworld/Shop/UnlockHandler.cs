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

		private UnlockHandler() {
			UnlockedUnits.Add(0);
			UnlockedUnits.Add(12);
			UnlockedUnits.Add(13);
		}

		public void UnlockUnitByID(int id) {
			if (UnlockedUnits.Contains(id)) {
				Debug.LogError("ID: "+id+" Was already unlocked!");
			}
			else {
				UnlockedUnits.Add(id);
			}
		}

		public List<int> GetUnlockedUnits() {
			return UnlockedUnits;
		}

		public int UnlockedCount() {
			return UnlockedUnits.Count;
		}
	}
}