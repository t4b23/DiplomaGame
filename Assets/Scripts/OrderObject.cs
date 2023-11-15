using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Sctriptable object/Order Object")]
public class OrderObject : ScriptableObject
{
    public Item[] orderedItems;
}