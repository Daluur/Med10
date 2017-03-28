using UnityEngine;

namespace Overworld {
	public class InGameMenu : ControlUIElement, IInteractable {

		void Start() {
			Register(this,KeyCode.Escape);
		}

		

		public void DoAction() {
			OpenElement();
		}

		public void DoAction<T>(T param) {
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}