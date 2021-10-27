using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptable : ScriptableObject
{
    /**The class that this Item represents. Only field that is not external data. */
    public string itemClassName;

    /**The sprite that represents this Item in the inventory. */
    public Sprite draggableSprite;

    /**The sprite for this item's inventory slot background. */
    public Sprite slotBackground;

    /**The sprite that represents this Item in the inventory when empty. */
    public Sprite greyDraggableSprite;

    /**The most basic sprite that appears when placed on a tile. */
    public Sprite basePlacedSprite;


    public AudioClip placeSound;


}
