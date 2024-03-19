using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Sctriptable object/Recipe Object")]
public class RecipeObject : ScriptableObject
{
    [Header("Only Gameplay")]
    public Item resultObject;
    public int duration;
    public Item[] components;
}