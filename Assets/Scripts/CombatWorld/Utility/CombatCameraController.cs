using System.Collections.Generic;
using CombatWorld.Units;
using CombatWorld.Map;
using UnityEngine;

namespace CombatWorld {
	public class CombatCameraController : Singleton<CombatCameraController> {

		float speed = 60.0f;
		Vector3 newPos;
		Vector2 minMaxXPos = new Vector2(-40, 40);
		public Vector3[] CamPositions;
		public Vector3[] CamPosRotations;
		int currentZoom = 1;

		bool AICam = false;
		bool following = false;
		Transform target;
		Vector3 targetPos;

		public bool moveToUnitClosestToEnemies = true;

		void Start() {
			transform.position = CamPositions[currentZoom] + new Vector3(transform.position.x, 0, 0);
			transform.eulerAngles = CamPosRotations[currentZoom];
		}
			
		void LateUpdate() {
			if (following) {
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

		public void setBoundary (Vector2 v) {
			minMaxXPos = v;
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
			if(target != null) {
				targetPos = target.transform.position;
			}
			newPos = transform.position;
			newPos.x = targetPos.x;
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
			this.target = target;
			following = true;
		}


		public void PlayerTurnsCam(List<Unit> Units) {
			Vector3 pos = Vector3.zero;
			if (Units.Count > 0) {
				if (moveToUnitClosestToEnemies) {
					pos = Units[0].transform.position;
					foreach (Unit unit in Units) {
						if(unit.transform.position.x > pos.x) {
							pos = unit.transform.position;
						}
					}
				}
				else {
					foreach (Unit unit in Units) {
						pos += unit.transform.position;
					}
					pos /= Units.Count;
				}
			}
			else {
				foreach (Node node in GameController.instance.GetPlayerSummonNodes()) {
					pos += node.transform.position;
				}
				pos /= 3;
			}
			target = null;
			targetPos = pos;
			following = true;
		}
	}
}