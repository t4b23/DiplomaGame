using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    public UpgradesManager upgradesManager;
    public PlayerControl playerControl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl = collision.GetComponent<PlayerControl>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerControl = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl != null)
        {
            if (playerControl.controler.PC.Interact.WasPressedThisFrame())
            {
                upgradesManager.OpenUpgradeWindow();
            }
        }
    }
}
