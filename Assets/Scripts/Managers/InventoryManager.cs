using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [Header("General")]
    public Controls controler;

    [Header("Inventory Slots")]
    public InventorySlot[] inventorySlots;
    int selectedSlot = -1;
    int notSelectedSlot;
    public GameObject NextItemToPickup;
    

    [Header("Prefabs and GameObjects")]
    public GameObject playerTransform;
    public GameObject inventoryItemPrefab;
    public GameObject itemToDropPrefab;
    public GameObject sellingPoint;
    public GameObject orderPlace;

    [Header("Craft")]
    public RecipeObject[] recipes;
    


    [Header("Order system")]
    public int numberOfItemsInOrder;
    public OrderManager orderManager;

    [Header("UI")]
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI moneyCounter;
    public int currentMoney;
    public int itemNumber = 0;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI[] itemsList;
    //int activeItemsInOrder;
    bool listActive;   

    private void Start()
    {
        ChangeSelectedSlot(0);
        notSelectedSlot = 1;
        turnOrderListOff();
        clearList();        
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
            notSelectedSlot = 1;
        }
        if (controler.PC.SelectSecondItem.WasPressedThisFrame())
        {
            ChangeSelectedSlot(1);
            notSelectedSlot = 0;
        }
        if (controler.PC.ScrollItems.WasPressedThisFrame())
        {
            if (selectedSlot == 0)
            {
                ChangeSelectedSlot(1);
                notSelectedSlot = 0;
            } else
            {
                ChangeSelectedSlot(0);
                notSelectedSlot = 1;
            }
        }
        if (controler.PC.Discard.WasPerformedThisFrame() && inventorySlots[selectedSlot].transform.childCount > 0)
        {
            DropItem(inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item);
        }
    }


    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        SetItemNameUI();
    }
    public void AddItem(Item item)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        InventorySlot freeSlot = inventorySlots[notSelectedSlot];
        InventoryItem itemInSecondSlot = freeSlot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null)
        {
            SpawnNewItem(item, slot);
            SetItemNameUI();
            return;
        } else if (itemInSecondSlot == null)
        {
            SpawnNewItem(item, freeSlot);
            return;
        }
    }


    private void SetItemNameUI()
    {
        if (inventorySlots[selectedSlot].hasItem)
        {
            Debug.Log("Current Item name is: " + inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().itemName);
            currentItemNameText.gameObject.SetActive(true);
            currentItemNameText.text = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().itemName;
        }
        else
        {
            Debug.Log("No Item in slot");
            currentItemNameText.gameObject.SetActive(false);
            currentItemNameText.text = null;
        }
    }
public bool HaveFreeSlot()
    {        
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        InventorySlot freeSlot = inventorySlots[notSelectedSlot];
        InventoryItem itemInSecondSlot = freeSlot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null)
        {
            return true;
        }
        else if (itemInSecondSlot == null)
        {
            return true;
        }
        else return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        slot.hasItem = true;
    }

    public void SellItem(GameObject order)
    {
        int money = System.Convert.ToInt32(moneyCounter.text);
        money += order.GetComponent<OrderObjectPrefabScript>().price;
        if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>() != null && order != null)
        {
            for (int i = 0; i < numberOfItemsInOrder; i++)
            {
                if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item == order.GetComponent<OrderObjectPrefabScript>().orderedItems[i])
                {
                    order.GetComponent<OrderObjectPrefabScript>().orderedItems[i] = null;
                    itemsList[i].text = null;
                    itemNumber++;
                    ClearSlot(inventorySlots[selectedSlot]);
                    break;
                }
            }

        }
        if (itemNumber == numberOfItemsInOrder)
        {
            currentMoney = money;
            moneyCounter.text = money.ToString();
            itemNumber = 0;
            sellingPoint.GetComponent<OrderPointScript>().OrderCompleted();
            sellingPoint.GetComponent<OrderPointScript>().currentOrder = null;
            Destroy(order);
            //orderManager.GenerateNewOrder();
            clearList();
            return;
        }
    }

    public void ChangeMoney(int money)
    {
        currentMoney = money;
        moneyCounter.text = money.ToString();
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
                RecipeObject checkRecipe = Instantiate(recipes[i],transform);
                for (int ind = 0; ind < checkRecipe.components.Length; ind++)
                {
                    if (itemInSlot.item == checkRecipe.components[ind] || nextitemInSlot.item == checkRecipe.components[ind])
                    {
                        checkRecipe.components[ind] = null;                        
                    }
                    if (checkRecipe.components[0] == null && checkRecipe.components[1] == null)
                    {
                        ClearInventory();
                        if (itemInSlot != null)
                        {
                            SpawnNewItem(checkRecipe.resultObject, slot);
                            //SetItemNameUI();
                            //Destroy(checkRecipe);
                            return;
                        }
                        else if (nextitemInSlot != null)
                        {
                            SpawnNewItem(checkRecipe.resultObject, nextslot);
                            //SetItemNameUI();
                            //Destroy(checkRecipe);
                            return;
                        }
                        else
                            return;
                    }
                }                
                Destroy(checkRecipe);
            }
            SetItemNameUI();
        }


    }

    public RecipeObject CheckRecipe(RecipeObject[] currentRecipes)
    {
        RecipeObject recipeToReturn = null;
        if (currentRecipes != null)
        {            
            InventorySlot slot = inventorySlots[selectedSlot];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            int craftComponents = 0;
            int j = 0;
            if (itemInSlot != null)
            {
                for (int ind = 0; ind < currentRecipes[j].components.Length; ind++)
                {
                    if (itemInSlot.item == currentRecipes[j].components[ind])
                    {
                        craftComponents++;
                        if (craftComponents == currentRecipes[j].components.Length)
                        {
                            if (itemInSlot.item == currentRecipes[j].components[ind])
                            {
                                ClearSlot(slot);
                                //SpawnNewItem(currentRecipe.resultObject, slot);
                                recipeToReturn = currentRecipes[j];
                            }
                            else
                                recipeToReturn = null;
                        }
                    }
                    if (j < currentRecipes.Length -1)
                    {
                        j++;
                    }
                }
            }
        }return recipeToReturn;
    } 

    public void ClearCurrentSlot()
    {
        ClearSlot(inventorySlots[selectedSlot]);
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
                slot.hasItem = false;
            }            
        }
        SetItemNameUI();
    }

    public void ClearSlot(InventorySlot slot)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Destroy(slot.transform.GetChild(0).gameObject);
            slot.hasItem = false;
            SetItemNameUI();
        }
        else return;        
    }

    public void SwitchOrderList()
    {
        if (listActive)
        {
            turnOrderListOff();
        }
        else
            turnOrderListOn();
    }

    public void turnOrderListOn()
    {
        orderPlace.SetActive(true);
        listActive = true;
        buttonText.text = "Hide list";
    }

    public void turnOrderListOff()
    {
        orderPlace.SetActive(false);
        listActive = false;
        buttonText.text = "Show list";
    }
    public void SetCurrentOrder(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            itemsList[i].text = items[i].name;
            itemsList[i].gameObject.SetActive(true);
            //activeItemsInOrder++;
        }
    }
    void clearList()
    {
        for (int i = 0; i < itemsList.Length; i++)
        {
            itemsList[i].text = null;
        }
    }

    public void DropItem(Item itemToDrop)
    {
        if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item != null)
        {
            Sprite itemImage = itemToDrop.image;
            GameObject dropppedObj = Instantiate(itemToDropPrefab, transform);
            dropppedObj.transform.position = playerTransform.transform.position;
            dropppedObj.GetComponent<DroppedItemScript>().droppedItem = itemToDrop;
            dropppedObj.GetComponent<SpriteRenderer>().sprite = itemImage;
            dropppedObj.GetComponent<DroppedItemScript>().inventoryManager = this;
            ClearSlot(inventorySlots[selectedSlot]);
        }

    }

    public void PickupItem()
    {
        if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>() == null && NextItemToPickup != null)
        {
            AddItem(NextItemToPickup.GetComponent<DroppedItemScript>().droppedItem);
            Destroy(NextItemToPickup);
            NextItemToPickup = null;
        }else if (inventorySlots[notSelectedSlot].GetComponentInChildren<InventoryItem>() == null && NextItemToPickup != null)
        {
            AddItem(NextItemToPickup.GetComponent<DroppedItemScript>().droppedItem);
            Destroy(NextItemToPickup);
            NextItemToPickup = null;
        }
    }
    
}
