using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStation : Station
{
    public override string NAME { get; } = "PlateStation";

    public override string[] ItemsCanTake { get; } = {
        "All" };

    public override float TimeToProcess { get; } = 1.0f;

    /**Set up ONE holder.*/
    public override void SetUpHolders()
    {
        Holders = new StationHolder[2];
        Holders[0] = transform.GetChild(0).GetComponent<StationHolder>();

    }
}