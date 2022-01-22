using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This class represents a basic Power Cell.</summary>
public class PowerCell : Item, IPlaceable, IConnectable
{
    public PowerCell() : base("PowerCell") { }

    public Vector2Int AttachRange()
    {
        return new Vector2Int(2,2);
    }

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

    public GameObject PrefabObject()
    {
        return Resources.Load<GameObject>("Prefabs/Power/PowerCell");
    }
}
