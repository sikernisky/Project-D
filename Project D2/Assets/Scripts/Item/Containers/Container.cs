using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents something that holds an Edible in game.
/// </summary>
public abstract class Container : Item
{
    public Container(string name) : base(name) { }

    public override void OnDropInventory(TileGrid grid)
    {
        Tile t = TileGrid.tileHovering;
        Structure s;
        if (t.IsOccupied()) s = t.Structure();
        else return;
        s.TakeItem(BoxItem());
    }

    
}
