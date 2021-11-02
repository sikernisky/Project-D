using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    /**Name of this Item. */
    public virtual string NAME { get; } = "Item";

    /**Report what this Item collided with. */
    public virtual void CollideInteraction(Item co)
    {
        Debug.Log(NAME + " collided with " + co.NAME);
    }

}
