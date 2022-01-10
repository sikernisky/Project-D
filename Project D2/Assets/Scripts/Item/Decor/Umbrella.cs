using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : Decor, IPlaceable
{
    public Umbrella() : base("Umbrella") { }

    public Vector2Int Bounds()
    {
        return new Vector2Int(2, 2);
    }

    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlace()
    {
        Debug.Log("Placed an Umbrella.");
    }


}
