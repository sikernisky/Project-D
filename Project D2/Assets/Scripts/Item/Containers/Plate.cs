using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a Plate moving around in the game.
/// </summary>
public class Plate : Container
{
    /// <summary>The Items on this Plate. </summary>
    private Item itemOnPlate;

    public Plate() : base("Plate") { }

    public override ItemBox BoxItem()
    {
        GameObject ob = new GameObject();
        ItemBox ib = ob.AddComponent<ItemBox>();
        ib.BoxItem(new Plate());
        return ib;
    }
}
