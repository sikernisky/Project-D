using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator
{
    public static Food GetFoodObject(string foodName)
    {
        GameObject food = new GameObject();

        switch (foodName) {
            case "Broccoli":
                return food.AddComponent<Broccoli>();
            default:
                return food.AddComponent<Food>();
        }
    }

    public static FoodScriptable GetFoodScriptableObject(string foodName)
    {
        try{ return (FoodScriptable)Resources.Load("FoodScriptables/" + foodName + "Scriptable"); }
        catch{
            Debug.Log("Caught");
            return (FoodScriptable)Resources.Load("FoodScriptables/DefaultFoodScriptable");}
    }
}
