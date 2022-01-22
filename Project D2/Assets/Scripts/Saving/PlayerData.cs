using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents data about the player. 
/// </summary>
public class PlayerData
{
    /// <summary>The level the player is on. Also represents the next level to load. </summary>
    public int currentLevel;

    /// <summary>The list of <c>Items</c> to load in the <c>Inventory</c>.</summary>
    public List<Item> nextInventory = new List<Item>() { new Strawberry(), new DefaultConveyor(), new CutStation(),
    new Umbrella(), new SquareTable(), new WoodenFence()};

    /// <summary>All unlocked Container Items. These will always be loaded into the next inventory. </summary>
    public List<Item> unlockedContainers = new List<Item>() { new Plate() };

}
