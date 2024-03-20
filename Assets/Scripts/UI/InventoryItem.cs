using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    public Item item;

    [Header("UI")]
    public Image image;
    public string itemName;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        itemName = newItem.itemName;
    }


}
