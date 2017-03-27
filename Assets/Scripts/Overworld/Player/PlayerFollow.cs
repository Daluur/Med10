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

		void Start () {
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
			cam.gameObject.transform.position = new Vector3(following.gameObject.transform.position.x + 12,yOffset + following.transform.position.y,following.transform.position.z - offSet);
			CameraZoom();
		}

		void CameraZoom() {
			var displacment = Input.mouseScrollDelta.y;
			if (displacment < 0f) {
				if(camPos.y + 1f  <= maxZoomOut){
					yOffset += 1f;
					offSet += 1f;
					camPos += new Vector3(0f, 1f, offSet);
				}
			}
			if (displacment > 0f) {
				if(( camPos.y - 1 ) > minZoomIn) {
					yOffset -= 1f;
					offSet -= 1f;
					camPos -= new Vector3(0f, 1f, offSet);
				}
			}
		}
	}
}