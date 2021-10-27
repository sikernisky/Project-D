using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeStation : Station
{
    public override string NAME { get; } = "ServeStation";

    public override string[] ItemsCanTake { get; } = {
        "All" };

    public override float TimeToProcess { get; } = 1.0f;

    /**Set up TWO holders: a left holder and a right holder. */
    public override void SetUpHolders()
    {
        Holders = new StationHolder[2];
        Holders[0] = transform.GetChild(0).GetComponent<StationHolder>();
        Holders[1] = transform.GetChild(1).GetComponent<StationHolder>();


    }



}
