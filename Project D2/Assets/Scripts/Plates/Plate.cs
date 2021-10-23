using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public List<Food> foodsOnPlate { get; }

    public void AddFoodToPlate(string foodToAdd)
    {
        gameObject.AddComponent(FoodGenerator.GetClassFromString(foodToAdd));
    }

    private void Update()
    {
        performAbilities();
    }

    private void performAbilities()
    {
        foreach(Food food in foodsOnPlate)
        {
            food.PerformAbility(this);
        }
    }

    public override string ToString()
    {
        string output = "";
        foreach(Food food in foodsOnPlate)
        {
            output += food.NAME + ", ";
        }
        return output;
    }


}
