using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderPointScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerControl playerControl;
    private bool interacting;
    public GameObject currentOrder;
    public GameObject sellingPoint;
    public OrderManager orderManager;
    public GameObject currentClient;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "Client" && currentClient == null)
        {
            Debug.Log("ClientInHitbox");
            currentClient = collision.gameObject;
        }

        if (collision.gameObject.tag == "Player")
        {
            playerControl = collision.GetComponent<PlayerControl>();
            interacting = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerControl = null;
            interacting = false;
        }

        if (collision.gameObject.tag == "Client" && currentClient != null)
        {
            currentClient = null;
        }
    }

    void Update()
    {
        if (playerControl != null && currentOrder != null)
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting && playerControl != null)
            {
                inventoryManager.numberOfItemsInOrder = currentOrder.GetComponent<OrderObjectPrefabScript>().orderedItems.Length;
                inventoryManager.SetCurrentOrder(currentOrder.GetComponent<OrderObjectPrefabScript>().orderedItems);
                sellingPoint.GetComponent<SellingPointScript>().currentOrder = currentOrder;
                currentClient.GetComponent<ClientLogic>().ChangePathToNew(sellingPoint.transform);
            }

    }
}
