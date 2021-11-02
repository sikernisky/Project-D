using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class FluidItem : Item, IMover
{
    /**Name of this FluidItem. */
    public override string NAME { get; } = "FluidItem";

    /** The GameTile this FluidItem is associated with. */
    public GameTile GameTileIn { get; protected set; }

    /** All items that can be dragged and dropped onto/into this FluidItem. */
    public abstract string[] ItemsCanTakeByDragging { get; }

    /** All items that can be moved onto/into this FluidItem from an IMover. */
    public abstract string[] ItemsCanTakeByMovement { get; }

    /**Return true if itemBeingDragged can be dragged to this FluidItem; false otherwise. */
    public bool CanDragTo(ItemScriptable itemBeingDragged)
    {
        if(ItemsCanTakeByDragging[0] == "All" || Array.IndexOf(ItemsCanTakeByDragging, itemBeingDragged.itemClassName) > -1){
            return true;
        }
        return false;
    }

    /**Return true if itemBeingDragged can be moved to this FluidItem; false otherwise. */
    public bool CanMoveTo(ItemScriptable itemBeingMoved)
    {
        if (ItemsCanTakeByMovement[0] == "All" || Array.IndexOf(ItemsCanTakeByMovement, itemBeingMoved.itemClassName) > -1)
        {
            return true;
        }
        return false;
    }

    /**Sets GameTileIn to gameTile if gameTile is not null. */
    public void AssignGameTile(GameTile gameTile)
    {
        if (gameTile != null) GameTileIn = gameTile;
    }

    public abstract void CashMovedItemIn(GameObject item);

    public abstract void DestroyMovedItem(GameObject item);



    /**Gives an item it holds to another IMover.*/
    public virtual void GiveMovedItem(GameObject item, FluidItem target)
    {
        if (target != null && GameTileIn.NextGameTile.TileSetUp)
        {
            target.TakeMovedItem(item);
        }
        else DestroyMovedItem(item);
    }

    public abstract void MoveItem(GameObject item);

    public abstract void TakeMovedItem(GameObject item);

    public abstract void TakeDraggedItem(ItemScriptable item);
}
