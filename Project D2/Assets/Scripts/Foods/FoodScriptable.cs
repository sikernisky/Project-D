using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food")]
public class FoodScriptable: ScriptableObject
{
    /**This exists to provide data. */
    public enum PathType { Sweet,Meat,Veggie}

    /**The Food that this Food represents. */
    public string foodClassName;

    /**The sprite that represents this Food in the inventory. */
    public Sprite draggableSprite;

    /**The sprite that represents this Food in the inventory when empty. */
    public Sprite greyDraggableSprite;

    /**The sound this object makes when placed. */
    public AudioClip placeSound;
    /**The most basic Em reward of this Food. */
    public int baseReward;

    /**The path type of this Food. */
    public PathType pathType;

}
