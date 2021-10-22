using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food")]
public class FoodScriptable: ScriptableObject
{
    /**This class exists to provide data that cannot be determined via script. */


    /**The Food that this Food represents. Only field that is not external data. */
    public string foodClassName;

    /**The sprite that represents this Food in the inventory. */
    public Sprite draggableSprite;

    /**The sprite for this item's inventory slot background. 
     * This is necessary because of shading purposes. */
    public Sprite slotBackground;

    /**The sprite that represents this Food in the inventory when empty. */
    public Sprite greyDraggableSprite;

    /**The sound this object makes when placed. */
    public AudioClip placeSound;


}
