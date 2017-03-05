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

		private bool isRunningShow, isRunningHide;

		void Start() {
			menu = GetComponent<Image>();
			myTexts = GetComponentsInChildren<Text>();
		}

		private void Update() {
			//if(Input.GetKeyDown(KeyCode.U))
				//DisplayMenu();
		}

		public void DisplayMenu(GameObject[] units) {
			if (isRunningShow) {
				return;
			}
			StartCoroutine(ShowMenu());
			PopulateUnitSlots(ConvertUnitsToStringArr(units));
		}

		public void CloseMenu() {
			if(isRunningHide)
				return;
			StartCoroutine(HideMenu());
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

		private void UnpopulateUnitSlots() {
			for (int i = 0; i < myTexts.Length; i++) {
				if(myTexts[i].tag != TagConstants.BUTTONTEXT)
					myTexts[i].text = "Empty";
			}
		}

		private IEnumerator ShowMenu() {
			isRunningShow = true;
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
			isRunningShow = false;
			yield return null;
		}

		private IEnumerator HideMenu() {
			isRunningHide = true;
			var startTime = Time.time;
			var initScale = new Vector2(menu.transform.localScale.x, menu.transform.localScale.y);
			var scalingLength = Vector2.Distance(initScale, Vector2.zero);
			var fracScaling = 0f;


			while (fracScaling < 1) {
				var distCovered = (Time.time - startTime) * speed;
				fracScaling = distCovered / scalingLength;
				var toScale = Vector2.Lerp(initScale, Vector2.zero, fracScaling);
				menu.transform.localScale = new Vector3(toScale.x, toScale.y, menu.transform.localScale.z);
				yield return new WaitForEndOfFrame();
			}
			UnpopulateUnitSlots();
			isRunningHide = false;
			yield return null;
		}

		private string[] ConvertUnitsToStringArr(GameObject[] units) {
			string[] tmp = new string[units.Length];
			for (int i = 0; i < units.Length; i++) {
				tmp[i] = units[i].GetComponent<Unit>().name;
			}
			return tmp;
		}



	}
}