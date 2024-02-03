using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingPointScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerControl playerControl;
    private bool interacting;
    public GameObject currentOrder;
    public GameObject currentClient;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl = collision.GetComponent<PlayerControl>();
        interacting = true;

        if (collision.gameObject.tag == "Client" && currentClient == null)
        {
            currentClient = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerControl = null;
        interacting = false;
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
                inventoryManager.SellItem(currentOrder);
                //currentClient.GetComponent<ClientLogic>().ExitBuilding();
                currentClient.GetComponent<ClientLogic>().gotOrder = true;
            }

    }

}
