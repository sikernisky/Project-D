using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemGenerator
{
    public static string DefaultConveyorPath { get; } = "Prefabs/ConveyorPrefabs/DefaultConveyorPrefab";
    public static string DeluxeConveyorPath { get; } = "Prefabs/ConveyorPrefabs/DeluxeConveyorPrefab";

    public static string PlateStationPath { get; } = "Prefabs/StationPrefabs/PlateStationPrefab";
    public static string ServeStationPath { get; } = "Prefabs/StationPrefabs/ServeStationPrefab";
    public static string RoastStationPath { get; } = "Prefabs/StationPrefabs/RoastStationPrefab";

    public static string SingleHorizontalFencePath { get; } = "Prefabs/ObstaclePrefabs/SingleHorizontalFencePrefab";
    public static string SingleVerticalFencePath { get; } = "Prefabs/ObstaclePrefabs/SingleVerticalFencePrefab";
    public static string SingleTopLeftFencePath { get; } = "Prefabs/ObstaclePrefabs/SingleTopLeftEndFencePrefab";
    public static string SingleBotLeftFencePath { get; } = "Prefabs/ObstaclePrefabs/SingleBotLeftEndFencePrefab";
    public static string SingleTopRightFencePath { get; } = "Prefabs/ObstaclePrefabs/SingleTopRightEndFencePrefab";
    public static string SingleBotRightFencePath { get; } = "Prefabs/ObstaclePrefabs/SingleBotRightEndFencePrefab";

    public static string SingleTopHorizontalFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleTopHorizontalFenceWoodPrefab";
    public static string SingleBotHorizontalFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleBotHorizontalFenceWoodPrefab";
    public static string SingleVerticalLeftFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleVerticalLeftFenceWoodPrefab";
    public static string SingleVerticalRightFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleVerticalRightFenceWoodPrefab";
    public static string SingleTopLeftFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleTopLeftFenceWoodPrefab";
    public static string SingleBotLeftFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleBotLeftEndFenceWoodPrefab";
    public static string SingleTopRightFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleTopRightFenceWoodPrefab";
    public static string SingleBotRightFenceWoodPath { get; } = "Prefabs/ObstaclePrefabs/SingleBotRightEndFenceWoodPrefab";

    public static string CustomerTablePath { get; } = "Prefabs/ObstaclePrefabs/CustomerTablePrefab";
        

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

    public static Item GetItemFromString(string itemName)
    {
        Type toAdd = GetClassFromString(itemName);
        GameObject transfer = new GameObject();
        transfer.AddComponent(toAdd);
        Item component = transfer.GetComponent<Item>();
        GameObject.Destroy(transfer);
        return component;
    }


}
