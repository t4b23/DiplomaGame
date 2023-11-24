using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : MonoBehaviour
{
    public Item droppedItem;

    public InventoryManager inventoryManager;

    public PlayerControl playerControl;

    private bool interacting;

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
        if (playerControl != null)
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting && playerControl != null)
            {
                inventoryManager.PickupItem();
            }

    }
}
