using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPointScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerControl playerControl;
    private bool interacting;
    public OrderObject order;
    public GameObject sellingPoint;
    //public OrderManager orderManager;

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
        if (playerControl != null && order != null)
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting && playerControl != null)
            {
                //orderManager.GenerateNewOrder();
                inventoryManager.numberOfItemsInOrder = order.orderedItems.Length;
                sellingPoint.GetComponent<SellingPointScript>().currentOrder = order;
            }

    }
}
