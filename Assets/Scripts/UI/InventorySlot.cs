using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public Sprite selectedColor, notSelectedColor;
    public bool hasItem;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.sprite = selectedColor;
    }

    public void Deselect()
    {
        image.sprite = notSelectedColor;
    }
}
