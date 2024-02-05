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
    public OrderManager orderManager;
    public bool gaveOrder;
    public GameObject currentClient;
    public ClientManager clientManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "Client" && currentClient == null)
        {
            Debug.Log("ClientInHitbox");
            currentClient = collision.gameObject;            
            currentOrder = currentClient.GetComponent<ClientLogic>().clientOrder;
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
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting && playerControl != null && !gaveOrder)
            {                
                inventoryManager.numberOfItemsInOrder = currentOrder.GetComponent<OrderObjectPrefabScript>().orderedItems.Length;
                inventoryManager.SetCurrentOrder(currentOrder.GetComponent<OrderObjectPrefabScript>().orderedItems);
                gaveOrder = true;
                //sellingPoint.GetComponent<SellingPointScript>().currentOrder = currentOrder;
                Debug.Log("Gave Order");
            }
            else if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting && playerControl != null && gaveOrder)
        {
                inventoryManager.SellItem(currentOrder);
        }        
    }

    public void OrderCompleted()
    {
        //currentOrder = null;
        currentClient.GetComponent<ClientLogic>().gotOrder = true;
        clientManager.ClientExit(currentClient);
        //clientManager.ManageQueue();
        gaveOrder = false;
        Debug.Log("sold");
    }
}
