using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;

namespace Overworld {
	public class CaveEntrance : ContextInteraction, IInteractable {

		void Start() {
			Register(this, KeyCode.Mouse0);
		}


		public void OWPopUpInformation() {
			
		}

		public void PerformClosenessAction() {

		}

		public void DoAction() {

		}

		public void DoAction<T>(T param) {
			CheckDistance();
		}

		public ControlUIElement GetControlElement() {
			return null;
		}
	}
}
