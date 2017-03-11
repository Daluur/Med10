using System;
using System.Collections;
using UnityEngine;

namespace Overworld {

	public class PlayerFollow : MonoBehaviour {

		public GameObject following;
		public float offSet = 12f;
		private Vector3 camPos;
		private Camera cam;
		public float maxZoom = 25.5f, minZoom = 5.5f;

		void Start () {
			cam = Camera.main;

			if (following == null) {
				following = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			}
			camPos = cam.gameObject.transform.position;
		}

		void Update () {
			cam.gameObject.transform.position = new Vector3(following.gameObject.transform.position.x,camPos.y,following.transform.position.z - offSet);
			CameraZoom();
		}

		void CameraZoom() {
			var displacment = Input.mouseScrollDelta.y;
			if (displacment < 0f) {
				if(camPos.y + 1f  <= maxZoom){
					offSet += 1f;
					camPos += new Vector3(0f, 1f, offSet);
				}
			}
			if (displacment > 0f) {
				if(( camPos.y - 1 ) > minZoom){
					offSet -= 1f;
					camPos -= new Vector3(0f, 1f, offSet);
				}
			}
		}
	}
}