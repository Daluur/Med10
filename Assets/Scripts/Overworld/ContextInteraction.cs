using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;

namespace Overworld {
	public class ContextInteraction : InputSubscriber {

		public GameObject playerOW;
		public bool isRunning = false;
		public bool meClicked = false;
		public float distanceToOpen = 5f;


		// Use this for initialization
	/*	void Start () {
			//if (playerOW == null) {
				playerOW = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			//}
		}
*/

		public void CheckDistance() {
			if (!isRunning)
				StartCoroutine(IsCloseEnough());
			//if(CanOpenMenu())
			//	OpenMenu();
		}

		private IEnumerator IsCloseEnough() {
			isRunning = true;
			while (meClicked) {
				if (DistanceBetweenObjAndPlayer()) {
					PerformClosenessAction();
					isRunning = false;
					yield break;
				}
				yield return new WaitForSeconds(0.3f);
			}
			isRunning = false;
			yield return null;
		}

		private bool DistanceBetweenObjAndPlayer() {
			if (Vector3.Distance(playerOW.transform.position, gameObject.transform.position) < distanceToOpen)
				return true;
			return false;
		}

		public virtual void PerformClosenessAction() {
			Debug.Log("Missing implementation of action when player is close enough to object");
		}


	}
}