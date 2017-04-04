using UnityEngine;

namespace CombatWorld {
	public class CombatCameraController : Singleton<CombatCameraController> {

		float speed = 60.0f;
		Vector3 newPos;
		Vector2 minMaxXPos = new Vector2(-40, 40);
		public Vector3[] CamPositions;
		public Vector3[] CamPosRotations;
		int currentZoom = 0;

		bool AICam = false;
		bool following = false;
		Transform AITarget;

		void Start() {
			transform.position = CamPositions[currentZoom] + new Vector3(transform.position.x, 0, 0);
			transform.eulerAngles = CamPosRotations[currentZoom];
		}
			
		void LateUpdate() {
			if (following && AITarget != null) {
				FollowCam();
			}
			if(!AICam) {
				PlayerControlledCam();
			}
			if (Input.mouseScrollDelta.y > 0) {
				//in
				currentZoom--;
				if (currentZoom < 0) {
					currentZoom = 0;
					return;
				}
				transform.position = CamPositions[currentZoom] + new Vector3(transform.position.x, 0, 0);
				transform.eulerAngles = CamPosRotations[currentZoom];
			}
			else if (Input.mouseScrollDelta.y < 0) {
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

		void PlayerControlledCam() {
			if (Input.GetMouseButton(0)) {
				if (Input.GetAxis("Mouse X") != 0) {
					newPos = transform.position - new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, 0, 0);
					newPos.x = Mathf.Clamp(newPos.x, minMaxXPos.x, minMaxXPos.y);
					transform.position = newPos;
					following = false;
				}
			}
		}

		Vector3 temp = Vector3.zero;

		void FollowCam() {
			newPos = transform.position;
			newPos.x = AITarget.position.x;
			transform.position = Vector3.SmoothDamp(transform.position, newPos, ref temp, 0.3f);
		}

		public void StartAICAM() {
			AICam = true;
			following = true;
		}

		public void EndAICAM() {
			AICam = false;
			following = false;
		}

		public void SetTarget(Transform target) {
			AITarget = target;
			following = true;
		}
	}
}