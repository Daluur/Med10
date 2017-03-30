using System;
using System.Collections;

using UnityEngine;

namespace Overworld {
	public class MenuUIController : MonoBehaviour {

		public float speed;
		public Vector2 size = new Vector2(1,1);
		[HideInInspector]
		public bool isRunning;
		[HideInInspector]
		public bool isShowing;

		public void OpenElement(GameObject go, Vector2 endScale) {
			if(isRunning)
				return;
			go.SetActive(true);
			StartCoroutine(OpenAUIElement(go, endScale));
		}

		public void CloseElement(GameObject go) {
			if(isRunning)
				return;
			StartCoroutine(OpenAUIElement(go, Vector2.zero));
		}

		private IEnumerator OpenAUIElement(GameObject go, Vector2 endScale) {

			isRunning = true;
			if (endScale != Vector2.zero) {
				isShowing = true;
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
			if (endScale == Vector2.zero) {
				isShowing = false;
			}
			isRunning = false;
			
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