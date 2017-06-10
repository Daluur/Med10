using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;

namespace SimplDynTut{
    public class OverworldTriggers : MonoBehaviour {
        public float notMovedTime = 5f;
        private static bool openedInventory, beenToShop, movedTopFourUnitsAround;
        
        void Start() {
            
            CheckMovementRequirements();
        }


        public static void MovedAroundUnitsToBringToBatlle() {
            movedTopFourUnitsAround = true;
        }
        
        public void ShowUnitsToBringToBatlleTutInfo() {
            if (!movedTopFourUnitsAround) {
                //TODO: Create correct tutorial information to be displayed
                Debug.LogWarning("The Move around your combat units trigger was hit, but there is currently no implementation for its tutorial information");
            }
        }

        public static void HasBeenToShop() {
            beenToShop = true;
        }
        
        public void ShowGoToShopTutInfo() {
            if (!beenToShop) {
                //TODO: Create correct tutorial information to be displayed
                Debug.LogWarning("The Go to shop trigger was hit, but there is currently no implementation for its tutorial information");
            }
        }

        public static void InventoryOpened() {
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
            yield return new WaitForSeconds(notMovedTime);
            TutorialHandler.instance.WorldTrigger(1);
        }
    }
}