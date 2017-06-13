using System;
using System.Collections;

using UnityEngine;

namespace Overworld {
	public class ControlUIElement : InputSubscriber {

		public float speed = 7;
		public Vector2 size = new Vector2(1,1);
		[HideInInspector]
		public bool isRunning;
		[HideInInspector]
		public bool isShowing;

		public void OpenElement() {
			if(isRunning)
				return;
			gameObject.SetActive(true);
			StartCoroutine(OpenAUIElement(gameObject, size, true));
		}

		public virtual void CloseElement() {
			if(isRunning)
				return;
			StartCoroutine(OpenAUIElement(gameObject, Vector2.zero, false));
		}

		private IEnumerator OpenAUIElement(GameObject go, Vector2 endScale, bool blockMouseUI) {
			isRunning = true;
			if (blockMouseUI && endScale != Vector2.zero) {
				inputManager.BlockMouseUI();
				isShowing = true;
				open = true;
			}
			var startTime = Time.time;
			var initScale = new Vector2(go.transform.localScale.x, go.transform.localScale.y);
			var scalingLength = Vector2.Distance(initScale, endScale);
			var fracScaling = 0f;

			while (fracScaling < 1) {
				var toScale = CalculateScalingUI(initScale, endScale, startTime, scalingLength, speed, out fracScaling);
				go.transform.localScale = new Vector3(toScale.x, toScale.y, go.transform.localScale.z);
				yield return new WaitForEndOfFrame();
			}
			isRunning = false;
			if (!blockMouseUI && endScale == Vector2.zero) {
				inputManager.UnblockMouseUI();
				isShowing = false;
				go.SetActive(false);
				open = false;
			}
			yield return null;

		}

		public static Vector2 CalculateScalingUI(Vector2 startScale, Vector2 endScale, float startTime, float length, float speed, out float fracScaling) {
			var distCovered = ( Time.time - startTime ) * speed;
			fracScaling = distCovered / length;
			var toScale = Vector2.Lerp(startScale, endScale, fracScaling);
			return toScale;
		}
	}
}
