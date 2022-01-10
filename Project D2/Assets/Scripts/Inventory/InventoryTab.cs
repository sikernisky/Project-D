using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum TabCat
{
    Edible,
    Placeable
}

/// <summary>
/// This class represents a category of the inventory.
/// </summary>
public class InventoryTab
{
    /// <summary> The items in this <c>InventoryTab</c>.</summary>
    private List<Item> items = new List<Item>();
    
    /// <summary> The name of this <c>InventoryTab</c>.</summary>
    private string tabName;

    /// <summary><strong>Constructor:</strong> an <c>InventoryTab</c> with name <c>name</c> and <c>items</c>.
    /// <br></br><em>Precondition:</em> <c>name</c> is not <c>null</c>. 
    /// <br></br><em>Precondition:</em> <c>items</c> is not <c>null</c> and has 0 < size < 7. </summary>
    public InventoryTab(string name, List<Item> items)
    {
        Assert.IsNotNull(name, "Parameter name cannot be null.");
        Assert.IsNotNull(items, "Parameter items cannot be null");
        Assert.IsTrue(items.Count > 0 && items.Count < 7, "Parameter items must have size > 0");

        tabName = name;
        this.items = items;
    }

    /// <summary><strong>Returns:</strong> the size of this <c>InventoryTab</c>.</summary>
    public int Size()
    {
        return items.Count;
    }

    /// <summary><strong>Returns:</strong> the name of this <c>InventoryTab</c>.</summary>
    public string Name()
    {
        return tabName;
    }

    /// <summary><strong>Returns:</strong> the items in this <c>InventoryTab</c>.</summary>
    public List<Item> Items()
    {
        return items;
    }



}
