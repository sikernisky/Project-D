using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTable : Table, IPlaceable
{
    public SquareTable() : base("SquareTable", 4) { }

    public override Vector2Int Bounds()
    {
        return new Vector2Int(3, 2);
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
