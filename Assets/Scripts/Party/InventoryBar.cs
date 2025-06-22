using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    [SerializeField]
    GameObject popup; 
    public void DisplayInventory()
    {
        popup.SetActive(true);
    }
}
