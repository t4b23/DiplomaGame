using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesManager : MonoBehaviour
{
    public GameObject UpgradeMenu;
    public bool isMenuOpen;  
    public InventoryManager inventoryManager;

    [Header("Coffee Components")]
    public CraftStationScript coffeMachine;
    public int[] coffeeTierPrice;
    public int coffeeTier;
    public TextMeshProUGUI coffeeButtonText;
    public TextMeshProUGUI coffeeInfoText;
    public Button coffeeButton;
    private bool maxCoffeeUpgrade = false;

    [Header("Milk Components")]
    public CraftStationScript milkMachine;
    public int[] milkTierPrice;
    public int milkTier;
    public TextMeshProUGUI milkButtonText;
    public TextMeshProUGUI milkInfoText;
    public Button milkButton;
    private bool maxMilkUpgrade = false;


    private void Start()
    {
        coffeeButtonText.text = coffeeTierPrice[coffeeTier].ToString() + "$";
        milkButtonText.text = milkTierPrice[milkTier].ToString() + "$";
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
        Debug.Log("TryingToUpgrade Coffee");
        if(inventoryManager.currentMoney >= coffeeTierPrice[coffeeTier] && isMenuOpen && !maxCoffeeUpgrade)
        {

            if (coffeeTier < coffeeTierPrice.Length)
            {
                inventoryManager.ChangeMoney(inventoryManager.currentMoney - coffeeTierPrice[coffeeTier]);
                coffeeTier++;
                if (coffeeTier == coffeeTierPrice.Length)
                {
                    coffeeInfoText.text = "- " + coffeeTier.ToString() + " seconds";
                    maxCoffeeUpgrade = true;
                    coffeMachine.durationBonus++;
                    coffeeButtonText.text = "MAX";
                    coffeeButton.interactable = false;
                    return;
                }
                coffeMachine.durationBonus++;
                coffeeInfoText.text = "- " + coffeeTier.ToString() + " seconds";
                coffeeButtonText.text = coffeeTierPrice[coffeeTier].ToString() + "$";

            }
        }
    }
    public void UpgradeMilkMachine()
    {
        Debug.Log("TryingToUpgrade Milk");
        if (inventoryManager.currentMoney >= milkTierPrice[milkTier] && isMenuOpen && !maxMilkUpgrade)
        {

            if (milkTier < milkTierPrice.Length)
            {
                inventoryManager.ChangeMoney(inventoryManager.currentMoney - milkTierPrice[milkTier]);
                milkTier++;
                if (milkTier == milkTierPrice.Length)
                {
                    milkInfoText.text = "- " + milkTier.ToString() + " seconds";
                    maxMilkUpgrade = true;
                    milkMachine.durationBonus++;
                    milkButtonText.text = "MAX";
                    milkButton.interactable = false;
                    return;
                }
                milkMachine.durationBonus++;
                milkInfoText.text = "- " + milkTier.ToString() + " seconds";
                milkButtonText.text = milkTierPrice[milkTier].ToString() + "$";

            }
        }
    }


}
