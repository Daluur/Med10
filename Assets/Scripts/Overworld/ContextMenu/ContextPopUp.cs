using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Overworld {

	public class ContextPopUp : ControlUIElement, IInteractable {

		private Text[] myUnitTexts;
		public Sprite defaultEmptyIcon;
		private List<Unit> units = new List<Unit>();
		private Button[] myButtons;
		private Image[] myIconImages;
		private Inventory inventory;


		public string buttonText = "Recruit for: ";

		void Start() {
			Register(this, KeyCode.Escape);
			InitUnitTexts(GetComponentsInChildren<Text>());
			InitRecruitButtons();
			myButtons = GetComponentsInChildren<Button>();
			InitUnitIconImages(GameObject.FindGameObjectsWithTag(TagConstants.ICONIMAGE));
			CloseElement(gameObject);
			inventory = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory>();
		}

		private void InitRecruitButtons() {
			var tmp = GameObject.FindGameObjectsWithTag(TagConstants.RECRUITBUTTON);
			myButtons = new Button[tmp.Length];
			for(int i=0;i<tmp.Length;i++) {
				myButtons[i] = tmp[i].GetComponent<Button>();
			}
		}

		private void InitUnitIconImages(GameObject[] myImages) {
			myIconImages = new Image[myImages.Length];
			for (int i = 0; i < myImages.Length; i++) {
				myIconImages[i] = myImages[i].GetComponent<Image>();
			}
		}

		private void InitUnitTexts(Text[] myTexts) {
			List<Text> ids = new List<Text>();
			foreach (var s in myTexts) {
				if (s.gameObject.tag.Equals(TagConstants.BUTTONTEXT)) {
					continue;
				}
				ids.Add(s);
			}
			myUnitTexts = ids.ToArray();
		}

		public void DisplayMenu(GameObject[] units) {
			if (isRunning || isShowing) {
				return;
			}
			UnpopulateUnitSlots();
			OpenElement(gameObject, size, true);
			PopulateUnitSlots(units);
		}

		public void CloseMenu() {
			if(isRunning || !isShowing)
				return;
			CloseElement(gameObject);
		}

		private void PopulateUnitSlots(GameObject[] units) {
			if (units.Length > myUnitTexts.Length) {
				Debug.LogError("There are too few icons to accomodate the different types of units, add more text elements to the context menu");
				return;
			}
			for(int i=0;i<units.Length;i++) {
				this.units.Add(units[i].GetComponent<Unit>());
				myUnitTexts[i].text = this.units[i].unitName;
				myIconImages[i].sprite = this.units[i].icon;
				AddListener(i, this.units[i]);

			}
		}

		private void UnpopulateUnitSlots() {
			for (int i = 0; i < myUnitTexts.Length; i++) {
				myUnitTexts[i].text = "Empty";
				myIconImages[i].sprite = defaultEmptyIcon;
				units.Clear();
			}
		}

		private void AddListener(int index, Unit unit) {
			myButtons[index].GetComponentInChildren<Text>().text = buttonText + unit.price;
			myButtons[index].onClick.AddListener(() => { AddItem(unit); });
		}

		private void AddItem(Unit unit) {
			if (!CurrencyHandler.RemoveCurrency(unit.price)) {
				return;
			}
			inventory.AddItem((int)unit.type);
		}

		public void DoAction() {
			CloseMenu();
		}

		public void DoAction<T>(T param) {
		}

		//Refactored, not using anymore, keeping just in case
		/*private IEnumerator ShowMenu() {
			isShowing = true;
			inputManager.BlockMouseUI();
			isRunningShow = true;
			var startTime = Time.time;
			var initScale = new Vector2(menu.transform.localScale.x, menu.transform.localScale.y);
			var scalingLength = Vector2.Distance(initScale, size);
			var fracScaling = 0f;


			while (fracScaling < 1) {
				var toScale = CalculateScalingUI(initScale, size, startTime, scalingLength, speed, out fracScaling);
				menu.transform.localScale = new Vector3(toScale.x, toScale.y, menu.transform.localScale.z);
				yield return new WaitForEndOfFrame();
			}
			isRunningShow = false;
			yield return null;
		}

		private IEnumerator HideMenu() {
			isShowing = false;
			isRunningHide = true;
			var startTime = Time.time;
			var initScale = new Vector2(menu.transform.localScale.x, menu.transform.localScale.y);
			var scalingLength = Vector2.Distance(initScale, Vector2.zero);
			var fracScaling = 0f;


			while (fracScaling < 1) {
				var toScale = CalculateScalingUI(initScale, Vector2.zero, startTime, scalingLength, speed, out fracScaling);
				menu.transform.localScale = new Vector3(toScale.x, toScale.y, menu.transform.localScale.z);
				yield return new WaitForEndOfFrame();
			}
			UnpopulateUnitSlots();
			inputManager.UnblockMouseUI();
			isRunningHide = false;
			gameObject.SetActive(false);
			yield return null;
		}*/

		/*private string[] ConvertUnitsToStringArr(GameObject[] units) {
			string[] tmp = new string[units.Length];
			for (int i = 0; i < units.Length; i++) {
				tmp[i] = units[i].GetComponent<Unit>().name;
			}
			return tmp;
		}*/
	}
}