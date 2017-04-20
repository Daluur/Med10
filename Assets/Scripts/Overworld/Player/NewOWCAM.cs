using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class NewOWCAM : MonoBehaviour {

		public Transform following;
		Vector3 offset;
		public Vector3 DefinedOffset = new Vector3(0,2.4f,3.6f);
		public float distanceToPlayer = 7;

		// Use this for initialization
		void Start() {
			if (following == null) {
				following = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER).transform;
			}
			if (DefinedOffset == Vector3.zero) {
				offset = transform.position - following.position;
				Debug.Log(offset);
			}
			else {
				offset = DefinedOffset;
			}
			if(distanceToPlayer == 0) {
				distanceToPlayer = offset.magnitude;
				Debug.Log(distanceToPlayer);
			}
			offset.Normalize();
		}

		// Update is called once per frame
		void LateUpdate() {
			transform.position = following.position + offset*distanceToPlayer;
		}
	}
}