using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour, IMover
{
    /** A list of Foods this Station can interact with */
    public abstract string[] ItemsCanTake { get; }

    /** The amount of time, in seconds, it takes to process a DEFAULT item. */
    public abstract float TimeToProcess { get; }

    /** Processes the item given, in whatever way that might be for this station. */
    public abstract void ProcessItem(GameObject itemToProcess, string itemName);

    /** Returns true of this Station can process the item. */
    public bool CanProcess(string itemName)
    {
        if (ItemsCanTake[0] == "All" || Array.IndexOf(ItemsCanTake, itemName) >= 0) return true;
        return false;
    }

    public abstract void TakeItem(GameObject item);

    public abstract void MoveItem(GameObject item);

    public abstract void GiveItem(GameObject item);

    public abstract void DestroyItem(GameObject item);

    public abstract void CashItemIn(GameObject item, int reward);
}
