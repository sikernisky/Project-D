using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

/// <summary>
/// Represents an Item used in game. 
/// </summary>
public abstract class Item
{
    /// <summary>Unique name of this item. </summary>
    private string itemName;

    /// <summary><strong>Returns:</strong> the unique Name of this item.</summary>
    public string Name()
    {
        return itemName;
    }

    public Item(string name)
    {
        Assert.IsNotNull(name, "Parameter name cannot be null.");
        itemName = name;
    }

    /// <summary><strong>Returns:</strong> the Sprite representing this <c>Item</c> in the Inventory.</summary>
    public Sprite InventorySprite()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Inventory/Components/" + itemName);
        return sprites[0];
    }

    /// <summary><strong>Returns:</strong> the Sprite representing this Item in an ItemBox.
    /// <br></br>By default, this is this Item's InventorySprite.</summary>
    public virtual Sprite ItemBoxSprite()
    {
        return InventorySprite();
    }

    /// <summary><strong>Returns:</strong> the slot Sprite backing this <c>Item</c> in the Inventory.</summary>
    public Sprite InventorySlot()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Inventory/Components/" + itemName);
        return sprites[2];
    }

    /// <summary>Do something when dragged from the Inventory.
    /// <br></br>(1) If this Item is an IPlaceable, show where it can be dropped on the grid.</summary>
    public void OnDragInventory(TileGrid grid)
    {
        if (this as IPlaceable == null) return;
        grid.ShowTileAvailability((IPlaceable)this);
    }

    /// <summary>Do something when dropped from the Inventory.
    /// <br></br>(1) If this Item is an IPlaceable, place it onto the grid. </summary>
    public virtual void OnDropInventory(TileGrid grid)
    {
        if (this as IPlaceable == null) return;
        grid.Place((IPlaceable)this, TileGrid.tileHovering);
    }


    /// <summary>Returns this Item wrapped in an ItemBox component.</summary>
    public abstract ItemBox BoxItem();
}
