using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Sctriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public TileBase tile;    
    public Vector2Int range = new Vector2Int(5, 4);  

    [Header("Only UI")]
    public bool sellable;
    public bool usedInCrafting;
    public string itemName;

    [Header("Both")]
    public Sprite image;
    public int Price;
}