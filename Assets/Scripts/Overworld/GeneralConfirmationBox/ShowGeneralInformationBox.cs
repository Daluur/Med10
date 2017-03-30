using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Overworld {
	public class ShowGeneralInformationBox : ContextInteraction, IInteractable {

		public ShowTypeGeneralInformation typeOfGeneralInformationTrigger;
		public TypeOfGeneralInformation typeOfGeneralInformation;
		public string displayText = "Empty", buttonOne = "BACKTEMPLATE", buttonTwo = "Next template";
		private ContextInteraction interaction;
		private UnityAction call;
		private bool hasShown = false;

		private void Start() {
			Register(this, KeyCode.Mouse0);

			var tmpCI = GetComponents<ContextInteraction>();
			foreach (var contextInteraction in tmpCI) {
				if (!contextInteraction.Equals(this)) {
					contextInteraction.hasGeneralConfirmationBox = true;
					call = () => contextInteraction.PerformClosenessAction();
				}
			}
			var tmpTrigger = GetComponents<EncounterArea>();
			foreach (var trigger in tmpTrigger) {
				switch (typeOfGeneralInformationTrigger) {
					case ShowTypeGeneralInformation.OnTriggerEnter:
						trigger.onTriggerEnter = false;
						call = () => trigger.DoOnTriggerEnter();
						break;
					case ShowTypeGeneralInformation.OnTriggerExit:
						trigger.onTriggerExit = false;
						call = () => trigger.DoOnTriggerExit();
						break;
				}
			}

		}

		public void DoAction() {
			meClicked = false;
		}

		public void DoAction<T>(T param) {
			if (hasShown)
				return;
			switch (typeOfGeneralInformationTrigger) {
				case ShowTypeGeneralInformation.IfClickedAndCloseEnough:
					meClicked = true;
					CheckDistance();
					break;
				case ShowTypeGeneralInformation.OnTriggerEnter:
					break;
				case ShowTypeGeneralInformation.OnTriggerExit:
					break;
				default:
					Debug.Log(typeOfGeneralInformationTrigger + " has not been implemented yet");
					break;
			}
		}

		public ControlUIElement GetControlElement() {
			return null;
		}

		public override void PerformClosenessAction() {
			ShowPopUp();

		}

		private void OnTriggerEnter(Collider other) {
			if(other.tag != TagConstants.OVERWORLDPLAYER && typeOfGeneralInformationTrigger != ShowTypeGeneralInformation.OnTriggerEnter)
				return;
			ShowPopUp();
		}


		private void OnTriggerExit(Collider other) {
			if(other.tag!= TagConstants.OVERWORLDPLAYER && typeOfGeneralInformationTrigger != ShowTypeGeneralInformation.OnTriggerExit)
				return;
			ShowPopUp();
		}

		private void ShowPopUp() {
			if (hasShown)
				return;
			if(typeOfGeneralInformation == TypeOfGeneralInformation.OneButton){
				GeneralConfirmationBox.instance.ShowPopUp(displayText, buttonOne, call);
			}
			else {
				GeneralConfirmationBox.instance.ShowPopUp(displayText, buttonOne, call, buttonTwo);
			}
			hasShown = true;
		}

		public enum ShowTypeGeneralInformation {
			OnTriggerEnter,
			OnTriggerExit,
			IfClickedAndCloseEnough
		}

		public enum TypeOfGeneralInformation {
			OneButton,
			TwoButtons
		}
	}
}
