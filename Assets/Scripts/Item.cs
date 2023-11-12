using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Sctriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public TileBase tile;    
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public Item[] craftingComponentOf;

    [Header("Only UI")]
    public bool stackable;
    public bool sellable;

    [Header("Both")]
    public Sprite image;
    public enum ItemType
    {
        Craftable,
        Tool,
        NonCraftable
    }

    public enum ActionType
    {
        Craft,
        Use,
        Sell
    }

}