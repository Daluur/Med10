using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatWorld.Utility;
using Overworld;
using UnityEngine;

namespace SimplDynTut{
    public class OverworldTriggers : Singleton<OverworldTriggers> {
        public float notMovedTime = 5f, dancedAroundTheEnemy = 15f;
        private static bool openedInventory, beenToShop, movedTopFourUnitsAround, enteredCombat, hasRunUnitsToBringToBattle;
        private Inventory inv;
        private List<Item> initOrder = new List<Item>();
        public ElementalTypes toCheckAgainstOne = ElementalTypes.Nature, toCheckAgainstTwo = ElementalTypes.Water;
        
        void Start() {
            inv = GetComponent<Inventory>();
            CheckMovementRequirements();
        }
        
        void Update() {
            if (initOrder.Count == 0) {
                initOrder = inv.GetFirstXItemsFromInventory(Values.NUMOFUNITSTOBRINGTOCOMBAT);
            }
        }

        public void MovedAroundUnitsToBringToBatlle() {
            movedTopFourUnitsAround = true;
        }
        
        public void ShowUnitsToBringToBatlleTutInfo() {
            if (!movedTopFourUnitsAround) {
                //TODO: Create correct tutorial information to be displayed
                Debug.LogWarning("The Move around your combat units trigger was hit, but there is currently no implementation for its tutorial information");
            }
        }

        public void HasBeenToShop() {
            beenToShop = true;
        }
        
        public void ShowGoToShopTutInfo() {
            if (!beenToShop) {
                //TODO: Create correct tutorial information to be displayed
                Debug.LogWarning("The Go to shop trigger was hit, but there is currently no implementation for its tutorial information");
            }
        }

        public void InventoryOpened() {
            openedInventory = true;
        }

        public void ShowInventoryOpenInformation() {
            if (!openedInventory) {
                //TODO: Create correct tutorial information to be displayed
                Debug.LogWarning("The inventory trigger was hit, but there is currently no implementation for its tutorial information");
            }
        }

        private void CheckMovementRequirements() {
            StartCoroutine(MovementReq());
        }

        private IEnumerator MovementReq() {
            if (PlayerData.Instance.GetHasPlayerMoved()) {
                yield break;
            }
			float timer = 0;
			while(timer < notMovedTime) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timer += Time.deltaTime;
				}
				yield return new WaitForEndOfFrame();
			}
            if (PlayerData.Instance.GetHasPlayerMoved()) {
                yield break;
            }
            TutorialHandler.instance.WorldTrigger(1);
        }

        public void StartEnterCombatTimer() {
            if(!enteredCombat)
                StartCoroutine(EnterCombatTimer());
        }

        private IEnumerator EnterCombatTimer() {
            enteredCombat = true;
			float timer = 0;
			while (timer < dancedAroundTheEnemy) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timer += Time.deltaTime;
				}
				yield return new WaitForEndOfFrame();
			}
			if (!PlayerData.Instance.hasEnteredCombat) {
				TutorialHandler.instance.WorldTrigger(3);
            }
        }

        public bool ChangedDeckBeforeEnteringCombat() {
            if(hasRunUnitsToBringToBattle)
                return true;
            hasRunUnitsToBringToBattle = true;
            var newOrder = inv.GetFirstXItemsFromInventory(Values.NUMOFUNITSTOBRINGTOCOMBAT);
            bool containsBothTypes = false;
            containsBothTypes = newOrder.Exists(e => e.Type.ToLower().Equals("water")) &&
                                newOrder.Exists(e => e.Type.ToLower().Equals("nature")); 
            if (!containsBothTypes) {
				TutorialHandler.instance.WorldTrigger(9);
                return false;
            }
            
            return true;
            
        }
    }
}