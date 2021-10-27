using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator
{
    /**Returns the ItemScriptable with the itemName. If not found, returns a default FoodScriptable. */
    public static ItemScriptable GetScriptableObject(string itemName)
    {
        if (Resources.Load("FoodScriptables/" + itemName + "Scriptable") != null)
        {
            return (FoodScriptable)Resources.Load("FoodScriptables/" + itemName + "Scriptable");
        }
        else if (Resources.Load("ConveyorScriptables/" + itemName + "Scriptable") != null)
        {
            return (ConveyorScriptable)Resources.Load("ConveyorScriptables/" + itemName + "Scriptable");
        }
        else if (Resources.Load("StationScriptables/" + itemName + "Scriptable") != null)
        {
            return (StationScriptable)Resources.Load("StationScriptables/" + itemName + "Scriptable");
        }
        Debug.Log("Couldn't find " + itemName);
        return (FoodScriptable)Resources.Load("FoodScriptables/DefaultFoodScriptable");
    }

    public static Type GetClassFromString(string itemName)
    {
        Type classFromString = Type.GetType(itemName);
        return classFromString;
    }
}
