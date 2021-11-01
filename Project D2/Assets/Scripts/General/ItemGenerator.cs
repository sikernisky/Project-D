using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator
{
    public static string DefaultConveyorPath { get; } = "Prefabs/ConveyorPrefabs/DefaultConveyorPrefab";
    public static string DeluxeConveyorPath { get; } = "Prefabs/ConveyorPrefabs/DeluxeConveyorPrefab";
    public static string PlateStationPath { get; } = "Prefabs/StationPrefabs/PlateStationPrefab";
    public static string ServeStationPath { get; } = "Prefabs/StationPrefabs/ServeStationPrefab";

    /**Returns the ItemScriptable with the itemName. If not found, returns a default FoodScriptable. */
    public static ItemScriptable GetScriptableObject(string itemName)
    {
        if (Resources.Load("Scriptables/FoodScriptables/" + itemName + "Scriptable") != null)
        {
            return (FoodScriptable)Resources.Load("Scriptables/FoodScriptables/" + itemName + "Scriptable");
        }
        else if (Resources.Load("Scriptables/ConveyorScriptables/" + itemName + "Scriptable") != null)
        {
            return (ConveyorScriptable)Resources.Load("Scriptables/ConveyorScriptables/" + itemName + "Scriptable");
        }
        else if (Resources.Load("Scriptables/StationScriptables/" + itemName + "Scriptable") != null)
        {
            return (StationScriptable)Resources.Load("Scriptables/StationScriptables/" + itemName + "Scriptable");
        }
        Debug.Log("Couldn't find " + itemName);
        return (FoodScriptable)Resources.Load("Scriptables/FoodScriptables/DefaultFoodScriptable");
    }

    public static Type GetClassFromString(string itemName)
    {
        Type classFromString = Type.GetType(itemName);
        return classFromString;
    }


}
