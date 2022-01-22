using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenFence : Decor, IPlaceable
{
    public WoodenFence() : base("WoodenFence") { }

    public Vector2Int Bounds()
    {
        return new Vector2Int(1, 1);
    }

    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlace()
    {
        return;
    }
}
