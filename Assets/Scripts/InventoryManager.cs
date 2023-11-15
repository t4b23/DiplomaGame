using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    public InventorySlot[] inventorySlots;
    public RecipeObject[] recipes;
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
        InventorySlot nextslot = inventorySlots[1];
        InventoryItem nextitemInSlot = nextslot.GetComponentInChildren<InventoryItem>();
        int craftComponents = 0;
        if (itemInSlot != null && nextitemInSlot != null)
        {
            for (int i = 0; i < recipes.Length; i++)
            {                
                for (int ind = 0; ind < recipes[i].components.Length; ind++)
                {
                    if (itemInSlot.item == recipes[i].components[ind] || nextitemInSlot.item == recipes[i].components[ind])
                    {
                        craftComponents++;
                        if (craftComponents == recipes[i].components.Length)
                        {
                            ClearInventory();
                            if (itemInSlot != null)
                            {
                                SpawnNewItem(recipes[i].resultObject, slot);
                                return;
                            }
                            else if (nextitemInSlot != null)
                            {
                                SpawnNewItem(recipes[i].resultObject, nextslot);
                                return;
                            }
                            else
                                return;
                        }
                    }
                }
            }
        }

    }

    public void UseCraftingStation(RecipeObject currentRecipe)
    {
        if (currentRecipe != null)
        {        
        InventorySlot slot = inventorySlots[0];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        InventorySlot nextslot = inventorySlots[1];
        InventoryItem nextitemInSlot = nextslot.GetComponentInChildren<InventoryItem>();
        int craftComponents = 0;
            if (itemInSlot != null || nextitemInSlot != null)
            {
                for (int ind = 0; ind < currentRecipe.components.Length; ind++)
                {
                    if (itemInSlot.item == currentRecipe.components[ind] || nextitemInSlot.item == currentRecipe.components[ind])
                    {
                        craftComponents++;
                        if (craftComponents == currentRecipe.components.Length)
                        {
                            if (itemInSlot.item == currentRecipe.components[ind])
                            {
                                ClearSlot(slot);
                                SpawnNewItem(currentRecipe.resultObject, slot);
                                return;
                            }
                            else if (nextitemInSlot.item == currentRecipe.components[ind])
                            {
                                ClearSlot(nextslot);
                                SpawnNewItem(currentRecipe.resultObject, nextslot);
                                return;
                            }
                            else
                                return;
                        }
                    }
                }
            }
        }
    }

    public void ClearInventory()
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

    public void ClearSlot(InventorySlot slot)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Destroy(slot.transform.GetChild(0).gameObject);
        }
        else return;
    }
}
