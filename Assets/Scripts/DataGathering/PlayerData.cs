using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

	#region Singleton Things

	private static PlayerData instance;

	public static PlayerData Instance {
		get {
			if (instance == null) {
				instance = new PlayerData();
			}
			return instance;
		}
	}

	#endregion

	public void HasPlayerMoved() {

	}
}
