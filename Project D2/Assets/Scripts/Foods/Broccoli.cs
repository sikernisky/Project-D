using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food
{

    public override FoodScriptable FoodScriptable { get; set; }

    private void Start()
    {

    }

    public override bool AbilityConditionMet()
    {
        return false;
    }

    public override void PerformAbility(Plate plate)
    {
        Debug.Log("Performing Broccoli's ability: printing the name of the plate it was passed: " + plate.ToString());
    }

}
