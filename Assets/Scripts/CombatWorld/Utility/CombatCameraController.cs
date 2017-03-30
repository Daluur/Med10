using UnityEngine;

namespace CombatWorld {
	public class CombatCameraController : MonoBehaviour {

		float speed = 60.0f;
		Vector3 newPos;
		Vector2 minMaxXPos = new Vector2(-40, 40);
		public Vector3[] CamPositions;
		public Vector3[] CamPosRotations;
		int currentZoom = 0;

		void Start() {
			transform.position = CamPositions[currentZoom] + new Vector3(transform.position.x, 0, 0);
			transform.eulerAngles = CamPosRotations[currentZoom];
		}

		void LateUpdate() {
			if (Input.GetMouseButton(0)) {
				if (Input.GetAxis("Mouse X") != 0) {
					newPos = transform.position - new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, 0, 0);
					newPos.x = Mathf.Clamp(newPos.x, minMaxXPos.x, minMaxXPos.y);
					transform.position = newPos;
				}
			}
			if(Input.mouseScrollDelta.y > 0) {
				//in
				currentZoom--;
				if (currentZoom < 0) {
					currentZoom = 0;
					return;
				}
				transform.position = CamPositions[currentZoom] + new Vector3(transform.position.x, 0, 0);
				transform.eulerAngles = CamPosRotations[currentZoom];
			}
			else if(Input.mouseScrollDelta.y < 0) {
				//out
				currentZoom++;
				if (currentZoom == CamPositions.Length) {
					currentZoom--;
					return;
				}
				transform.position = CamPositions[currentZoom] + new Vector3(transform.position.x, 0, 0);
				transform.eulerAngles = CamPosRotations[currentZoom];
			}
		}
	}
}