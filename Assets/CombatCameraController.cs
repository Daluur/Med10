using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCameraController : MonoBehaviour {

	float speed = 20.0f;

	void LateUpdate() {
		if (Input.GetMouseButton(1)) {
			if (Input.GetAxis("Mouse X") != 0) {
				transform.position -= new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, 0, 0);
			}
		}
	}
}
