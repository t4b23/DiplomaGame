using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPointScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerControl playerControl;
    private bool interacting;
    public GameObject currentOrder;
    public GameObject sellingPoint;
    public OrderManager orderManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl = collision.GetComponent<PlayerControl>();
        interacting = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerControl = null;
        interacting = false;
    }

    void Update()
    {
        if (playerControl != null && currentOrder != null)
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting && playerControl != null)
            {
                inventoryManager.numberOfItemsInOrder = currentOrder.GetComponent<OrderObjectPrefabScript>().orderedItems.Length;
                inventoryManager.SetCurrentOrder(currentOrder.GetComponent<OrderObjectPrefabScript>().orderedItems);
                sellingPoint.GetComponent<SellingPointScript>().currentOrder = currentOrder;
            }

    }
}
