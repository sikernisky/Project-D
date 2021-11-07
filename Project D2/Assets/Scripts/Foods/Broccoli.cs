using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food
{
    public override int BASE_REWARD { get; } = 50;

    public override string NAME { get; } = "Broccoli";

    public override PathType PATH_TYPE { get; } = PathType.Veggie;

    public override bool Roasted { get; set; }
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
