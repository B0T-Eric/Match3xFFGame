using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface
{
    public class EndCombatScreen : MonoBehaviour
    {
        [SerializeField]
        Button skipButton, confirmButton;
        [SerializeField]
        public Reward[] rewards;
        private GameObject selectedItem;
        public void SetSelectedItem(GameObject item) { selectedItem = item;}
        private void Start()
        {
            GameObject.Find("GameContainer").SetActive(false);
            for (int i = 0; i < rewards.Length; i++)
            {
                Reward reward = rewards[i];
                reward.Init(this);
            }
            confirmButton.interactable = false;
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmClick);
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(LoadMap);
        }
        public void ConfirmClick()
        {
            EquipItem();
            LoadMap();
        }
        public void LoadMap()
        {
            ContinueClick.LoadMap();
        }
        public void EquipItem()
        {
            foreach (BaseCharacter c in Party.Instance.members)
            {
                foreach (CharacterInventorySlot slot in c.inventorySlots)
                {
                    bool isSlotFree = !slot.isOccupied && slot.transform.childCount == 0;
                    if (isSlotFree)
                    {
                        slot.DisplayEquipedItem(selectedItem);
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            Debug.Log("Didn't Find Empty Slot");
        }

        internal void ChangeButtonVisual(Reward r)
        {
            foreach (Reward reward in rewards)
            {
                if(reward != r)
                {
                    reward.selected = false;
                }
                else
                {
                    if(!r.selected)
                    r.selected = true;
                }
                reward.UpdateVisuals();
            }
            confirmButton.interactable = true;
        }
    }
}