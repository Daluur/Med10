using System;
using System.Collections;
using UnityEngine;

namespace Overworld {

	public class PlayerFollow : MonoBehaviour {

		public GameObject following;
		public float offSet = 12f;
		public float yOffset = 20f;
		private Vector3 camPos;
		private Camera cam;
		public float maxZoom = 25.5f, minZoom = 5.5f;
		private float maxZoomOut, minZoomIn;
		private InputManager inputManager;
		public float speed = 4;

		void Start () {
			inputManager = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<InputManager>();
			cam = Camera.main;

			if (following == null) {
				following = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			}
			camPos = cam.gameObject.transform.position;
			camPos = new Vector3(camPos.x, yOffset + following.transform.position.y, camPos.z);
			maxZoomOut = camPos.y + maxZoom;
			minZoomIn = camPos.y - minZoom;
		}

		void Update () {
			cam.gameObject.transform.position = new Vector3(following.gameObject.transform.position.x + offSet/8,yOffset + following.transform.position.y,following.transform.position.z - offSet);
			CameraZoom();
		}

		void CameraZoom() {
			if (inputManager.GetMouseBlocked()) {
				return;
			}
			var displacment = Input.mouseScrollDelta.y;
			var direction = (following.transform.position - camPos).normalized;
			var offsetSpeeded = 1f * speed;
			direction *= speed;

			if (displacment < 0f) {
				Debug.Log("zoom out");
				if(camPos.y + 1f  <= maxZoomOut){
					yOffset -= direction.y;
					offSet -= offsetSpeeded;
					camPos -= direction;
				}
			}
			if (displacment > 0f) {
				Debug.Log("zoom in");
				if(( camPos.y - 1 ) > minZoomIn) {
					yOffset += direction.y;
					offSet += offsetSpeeded;
					camPos += direction;
				}
			}








		}
	}
}