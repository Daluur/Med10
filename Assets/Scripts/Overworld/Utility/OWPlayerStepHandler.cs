using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWPlayerStepHandler : MonoBehaviour {

	public void Step () {
		AudioHandler.instance.PlayMove();	
	}
}
