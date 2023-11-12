using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    public InventorySlot[] inventorySlots;
    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i]; 
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item,slot);
                return;
            }
        }
    }

    void SpawnNewItem (Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }


    public void CraftItem()
    {
        InventorySlot slot = inventorySlots[0];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        InventorySlot nextndslot = inventorySlots[1];
        InventoryItem nextitemInSlot = slot.GetComponentInChildren<InventoryItem>();
        
        for (int i = 0; i < itemInSlot.item.craftingComponentOf.Length; i++)
        {
            Item firstComponent = itemInSlot.item.craftingComponentOf[i];
            for (int ind = 0; i < nextitemInSlot.item.craftingComponentOf.Length; i++)
            {
                if (firstComponent == nextitemInSlot.item.craftingComponentOf[ind])
                {
                    ClearInventory();
                    SpawnNewItem(firstComponent, inventorySlots[0]);
                    return;
                }
            }
           
        }
    }

    void ClearInventory()
    {
        for (int iter = 0; iter < inventorySlots.Length; iter++)
        {
            InventorySlot slot = inventorySlots[iter];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(slot.transform.GetChild(0).gameObject);                 
            }
            else return;
        }
    }
}
