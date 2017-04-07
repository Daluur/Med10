using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Overworld;
using UnityEngine;
using UnityEngine.AI;

namespace Overworld {
	public class ContextInteraction : InputSubscriber {

		[HideInInspector]
		public GameObject playerOW;
		[HideInInspector]
		public bool isRunning = false;
		[HideInInspector]
		public bool meClicked = false;
		float distanceToOpen = Values.InteractDistance;
		[HideInInspector]
		public bool hasGeneralConfirmationBox = false;

		protected override void Awake() {
			AssignPlayer();
			base.Awake();
		}

		public void AssignPlayer() {
			playerOW = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
		}

		public void CheckDistance() {
			if (!isRunning)
				StartCoroutine(IsCloseEnough());
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
			if (Vector3.Distance(playerOW.transform.position, gameObject.transform.position) < distanceToOpen){
				playerOW.GetComponent<PlayerMovementOW>().DoAction();
				return true;
			}
			return false;
		}

		public virtual void PerformClosenessAction() {
			Debug.Log("Missing implementation of action when player is close enough to object");
		}

		public static Vector2 CalculateScalingUI(Vector2 startScale, Vector2 endScale, float startTime, float length, float speed, out float fracScaling) {
			var distCovered = ( Time.time - startTime ) * speed;
			fracScaling = distCovered / length;
			var toScale = Vector2.Lerp(startScale, endScale, fracScaling);
			return toScale;
		}

		protected virtual void OnMouseEnter() {
			CursorSingleton.instance.SetCursor(gameObject);
		}

		protected virtual void OnMouseExit() {
			CursorSingleton.instance.SetCursor();

		}
	}
}