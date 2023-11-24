using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

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
    public TextMeshProUGUI moneyCounter;
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
            return;
        } else if (itemInSecondSlot == null)
        {
            SpawnNewItem(item, freeSlot);
            return;
        }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
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
            moneyCounter.text = money.ToString();
            itemNumber = 0;
            sellingPoint.GetComponent<SellingPointScript>().currentOrder = null;
            Destroy(order);
            orderManager.GenerateNewOrder();
            clearList();
            return;
        }
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
