using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CraftStationScript : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public RecipeObject recipeToCraft;

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
        if(playerControl != null)
        {
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && interacting)
            {
                inventoryManager.UseCraftingStation(recipeToCraft);
            }
        }
    }
}
