using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStation : Station
{
    public override string[] ItemsCanTake { get; } = {
        "GroundBeef",
        "MarinatedCarnitas",
        "SlicedStrawberry",
        "BananaChunks" };

    public override float TimeToProcess { get; } = 0f;

    public override void CashItemIn(GameObject item, int reward)
    {
        throw new System.NotImplementedException();
    }

    public override void DestroyItem(GameObject item)
    {
        throw new System.NotImplementedException();
    }

    public override void GiveItem(GameObject item)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveItem(GameObject item)
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessItem(GameObject itemToProcess, string itemName)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeItem(GameObject item)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
