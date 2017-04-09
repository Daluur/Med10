using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld;

public class BillboardCanvas : MonoBehaviour {

	Camera cam;
	
	// Update is called once per frame
	void Update () {
		if(cam == null) {
			cam = Camera.main.GetComponent<CombatCameraController>() == null ? null : Camera.main;
			return;
		}
		transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
	}
}
