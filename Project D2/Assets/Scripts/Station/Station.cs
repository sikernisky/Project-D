using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    /** A list of Foods this Station can interact with */
    public abstract List<string> ItemsCanTake { get; set; }

    /** The amount of time, in seconds, it takes to process a DEFAULT item. */
    public abstract int TimeToProcess { get; set; }

    /** Processes the item given, in whatever way that might be for this station. */
    public abstract void ProcessItem(GameObject itemToProcess, string itemName);

    /** Returns true of this Station can process the item. */
    public bool CanProcess(string itemName)
    {
        if (!ItemsCanTake.Contains(itemName)) return false;
        return true;
    }

}
