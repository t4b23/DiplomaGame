using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    public GameObject sellingPoint;
    public InventorySlot[] inventorySlots;
    public RecipeObject[] recipes;
    int selectedSlot = -1;
    public Controls controler;
    public TextMeshProUGUI moneyCounter;
    public int itemNumber = 0;
    public OrderManager orderManager;
    public int numberOfItemsInOrder;
    private void Start()
    {
        ChangeSelectedSlot(0);        
    }

    private void Awake()
    {
        controler = new Controls();
    }

    private void OnEnable()
    {
        controler.Enable();
    }

    private void OnDisable()
    {
        controler.Disable();
    }

    private void Update()
    {
        if (controler.PC.SelectFirstItem.WasPressedThisFrame())
        {
            ChangeSelectedSlot(0);
        }
        if (controler.PC.SelectSecondItem.WasPressedThisFrame())
        {
            ChangeSelectedSlot(1);
        }
        if (controler.PC.ScrollItems.WasPressedThisFrame())
        {
            if (selectedSlot == 0)
            {
                ChangeSelectedSlot(1);
            }else
                ChangeSelectedSlot(0);
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }        

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    public void AddItem(Item item)
    {
            InventorySlot slot = inventorySlots[selectedSlot]; 
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item,slot);
                return;
            }    
    }

    void SpawnNewItem (Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void SellItem(OrderObject order)
    {
        //numberOfItemsInOrder = order.orderedItems.Length;
        int money = System.Convert.ToInt32(moneyCounter.text);
        money += order.price;
        if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>() != null && order != null)
        {
            for(int i = 0; i < numberOfItemsInOrder; i++)
            {
                if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item == order.orderedItems[i])
                {
                    //order.placedItemsOnDesk[itemNumber] = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item;
                    itemNumber++;
                    ClearSlot(inventorySlots[selectedSlot]);                   
                }
            }
            if (itemNumber == numberOfItemsInOrder)
            {
                moneyCounter.text = money.ToString();
                itemNumber = 0;
                sellingPoint.GetComponent<SellingPointScript>().currentOrder = null;
                orderManager.GenerateNewOrder();
                return;
            }
        }
        return;
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
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        int craftComponents = 0;
            if (itemInSlot != null)
            {
                for (int ind = 0; ind < currentRecipe.components.Length; ind++)
                {
                    if (itemInSlot.item == currentRecipe.components[ind])
                        
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
