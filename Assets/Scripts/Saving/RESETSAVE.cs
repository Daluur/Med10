using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RESETSAVE : MonoBehaviour {

	public void RESET() {
		SaveLoadHandler.Instance.Reset();
	}
}
