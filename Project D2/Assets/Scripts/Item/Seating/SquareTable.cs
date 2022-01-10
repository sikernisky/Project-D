using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTable : Table, IPlaceable
{
    public SquareTable() : base("SquareTable", 4) { }

    public override Vector2Int Bounds()
    {
        throw new System.NotImplementedException();
    }

    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlace()
    {
        Debug.Log("Placed a " + Name());
    }
}
