using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Overworld {

	public class ContextPopUp : MonoBehaviour {

		private Image menu;
		public Vector2 size = new Vector2(10f, 20f);
		public float speed = 1.0f;
		private Text[] myTexts;

		private Coroutine isRunning;

		void Start() {
			menu = GetComponent<Image>();
			myTexts = GetComponentsInChildren<Text>();
		}

		private void Update() {
			//if(Input.GetKeyDown(KeyCode.U))
				//DisplayMenu();
		}

		public void DisplayMenu(string[] units) {
			if(isRunning!=null)
				return;
			isRunning = StartCoroutine(ShowMenu());
			PopulateUnitSlots(units);
		}

		private void PopulateUnitSlots(string[] units) {
			if (units.Length > myTexts.Length) {
				Debug.LogError("There are too few icons to accomodate the different types of units, add more text elements to the context menu");
				return;
			}
			for(int i=0;i<units.Length;i++) {
				myTexts[i].text = units[i];
			}
		}

		private IEnumerator ShowMenu() {
			var startTime = Time.time;
			var initScale = new Vector2(menu.transform.localScale.x, menu.transform.localScale.y);
			var scalingLength = Vector2.Distance(initScale, size);
			var fracScaling = 0f;


			while (fracScaling < 1) {
				var distCovered = (Time.time - startTime) * speed;
				fracScaling = distCovered / scalingLength;
				var toScale = Vector2.Lerp(initScale, size, fracScaling);
				menu.transform.localScale = new Vector3(toScale.x, toScale.y, menu.transform.localScale.z);
				yield return new WaitForEndOfFrame();
			}
			yield return null;
		}

	}
}