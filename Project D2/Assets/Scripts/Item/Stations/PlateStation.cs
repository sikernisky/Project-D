using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStation : StationInventory, IPlaceable, IMoveable, IConnectable
{

    public PlateStation() : base("PlateStation", new Vector2Int(2, 3)) { }

    public override Vector2Int AttachRange()
    {
        return new Vector2Int(1, 1);
    }

    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }

    public override Direction Direction()
    {
        throw new System.NotImplementedException();
    }

    public override Sprite[] MoveAnimation()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAccept()
    {
        throw new System.NotImplementedException();
    }

    public override void OnClick()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlace()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject PrefabObject()
    {
        throw new System.NotImplementedException();
    }
}
