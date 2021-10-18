using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Food")]
public class FoodScriptable: ScriptableObject
{
    public Food foodClass;

    public Sprite inventorySprite;

    public Sprite greyInventorySprite;

    public AudioClip placeSound;

}
