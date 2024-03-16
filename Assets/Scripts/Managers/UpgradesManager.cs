using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesManager : MonoBehaviour
{
    public GameObject UpgradeMenu;
    public bool isMenuOpen;
    public CraftStationScript coffeMachine;
    public InventoryManager inventoryManager;
    public int[] coffeeTierPrice;
    public int coffeeTier;
    public TextMeshProUGUI coffeeButtonText;
    public TextMeshProUGUI coffeeInfoText;
    public Button coffeeButton;
    private bool maxCoffeeupgrade = false;

    private void Start()
    {
        coffeeButtonText.text = coffeeTierPrice[coffeeTier].ToString() + "$";
    }

    public void OpenUpgradeWindow()
    {
        UpgradeMenu.SetActive(true);
        isMenuOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseUpgradeWindow()
    {
        UpgradeMenu.SetActive(false);
        isMenuOpen = false;
        Time.timeScale = 1f;
    }
    public void UpgradeCoffeeMachine() 
    {
        Debug.Log("TryingToUpgrade");
        if(inventoryManager.currentMoney >= coffeeTierPrice[coffeeTier] && isMenuOpen && !maxCoffeeupgrade)
        {

            if (coffeeTier < coffeeTierPrice.Length)
            {
                inventoryManager.ChangeMoney(inventoryManager.currentMoney - coffeeTierPrice[coffeeTier]);
                coffeeTier++;
                if (coffeeTier == coffeeTierPrice.Length)
                {
                    coffeeInfoText.text = "- " + coffeeTier.ToString() + " seconds";
                    maxCoffeeupgrade = true;
                    coffeeButtonText.text = "MAX";
                    coffeeButton.interactable = false;
                    return;
                }
                coffeMachine.duration = coffeMachine.duration - 1;
                coffeeInfoText.text = "- " + coffeeTier.ToString() + " seconds";
                coffeeButtonText.text = coffeeTierPrice[coffeeTier].ToString() + "$";

            }
        }
    }
        
}
