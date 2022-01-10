using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CutStationInventory : StationInventory
{
    public CutStationInventory() : base("CutStation", new Vector2Int(3,2)) { }

    public override Direction Direction()
    {
        throw new System.NotImplementedException();
    }

    public override Sprite[] MoveAnimation()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlace()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject PrefabObject()
    {
        GameObject ob =  Resources.Load<GameObject>("Prefabs/Stations/CutStation");
        Assert.IsNotNull(ob.GetComponent<Structure>(), "The Prefab for " + Name() + " must contain a Structure component.");
        return ob;
    }

    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAccept()
    {
        return;
    }

    public override void OnClick()
    {
        return;
    }

    public override int AttachRange()
    {
        return 1;
    }
}
