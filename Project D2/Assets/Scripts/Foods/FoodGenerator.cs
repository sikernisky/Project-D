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
        switch (foodName) {
            case "Broccoli":
                return (FoodScriptable)Resources.Load("FoodScriptables/BroccoliScriptable");
            default:
                return (FoodScriptable)Resources.Load("FoodScriptables/DefaultFoodScriptable");
        }

    }
}
