using UnityEngine;

namespace Overworld {
	public class InGameMenu : ControlUIElement, IInteractable {

		private bool buttonOpened = false;

		void Start() {
			Register(this,KeyCode.Escape);
		}

		public void Button() {
			inputManager.FakeInput(KeyCode.Escape);
		}


		public void DoAction() {
			if(!isRunning || !isShowing){
				OpenElement();
			}
		}

		public void CloseMenu() {
			inputManager.inGameMenuOpen = false;
			buttonOpened = false;
			CloseElement ();
		}

		public void DoAction<T>(T param) {
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}