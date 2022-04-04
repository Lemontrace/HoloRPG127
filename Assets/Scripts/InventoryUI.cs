using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
 public GameObject InventoryPanel;
 bool activeInventory = false;

 private void Start()
    {
        InventoryPanel.SetActive(activeInventory);
    }
 private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            InventoryPanel.SetActive(activeInventory);
        }
    }
}
