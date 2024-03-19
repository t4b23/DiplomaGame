using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CraftStationScript : MonoBehaviour
{
    public int durationBonus;
    private int SmallEpsilon = 1;


    public InventoryManager inventoryManager;

    public RecipeObject[] recipesToCraft;

    public PlayerControl playerControl;

    public ProgressBar progressBar;
    private bool readyToGive = false;
    private Item itemToGive;
    public Image readyIcon;


    //private bool interacting;
    public bool inProgress = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl = collision.GetComponent<PlayerControl>();
        //interacting = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerControl = null;
        //interacting = false;
    }

    void Update()
    {
        if(playerControl != null)
        {
            if (playerControl.controler.PC.Interact.WasPressedThisFrame() && !inProgress && !readyToGive && itemToGive == null)
            {
                RecipeObject currentRecipe = inventoryManager.CheckRecipe(recipesToCraft);
                //inventoryManager.ClearCurrentSlot();
                if (currentRecipe != null)
                {
                    itemToGive = currentRecipe.resultObject;
                    int duration = currentRecipe.duration;
                    duration = duration - durationBonus;
                    StartCoroutine(progressTime(duration));
                }

            }else if(playerControl.controler.PC.Interact.WasPressedThisFrame() && !inProgress && readyToGive && itemToGive != null && inventoryManager.HaveFreeSlot())
            {
                inventoryManager.AddItem(itemToGive);
                readyIcon.gameObject.SetActive(false);
                itemToGive = null;
                readyToGive=false;
            }
        }
    }


    IEnumerator progressTime(int duration)
    {
        inProgress = true;
        progressBar.turnOn();
        float time = 0.0f;
        while (time < duration)
        {
            progressBar.changeSliderValue(time / Mathf.Max(duration, SmallEpsilon));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        readyIcon.gameObject.SetActive(true);
        progressBar.changeSliderValue(0);
        progressBar.turnOff();
        readyToGive = true;
        inProgress = false;

    }

}
