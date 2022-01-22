using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

/// <summary>
/// This class manages the Inventory and its tabs.
/// </summary>
public class InventoryMaster: MonoBehaviour
{
    /// <summary>The slots in this Inventory: there are always six. </summary>
    [SerializeField]
    private InventorySlot[] slots = new InventorySlot[6];

    /// <summary>The tabs above this Inventory: there are always six. </summary>
    [SerializeField]
    private TabButton[] tabs = new TabButton[6];

    /// <summary>The TileGrid this Inventory drags onto.</summary>
    private TileGrid grid;

    /// <summary>The <c>InventoryTabs</c> in this Inventory.</summary>
    private static List<InventoryTab> activeTabs = new List<InventoryTab>();


    private void Start()
    {
        ConstructTabs(SaveMaster.Inventory());
        BecomeSlotMaster();
    }

    /// <summary>Constructs each <c>InventoryTab</c> in this Inventory.</summary>
    private void ConstructTabs(List<Item> inv)
    {
        int tabCounter = 0;

        InventoryTab edibleTab = CreateEdibleTab(inv);
        if (edibleTab != null)
        {
            activeTabs.Add(edibleTab);
            tabs[tabCounter].Activate(edibleTab);
            tabCounter++;
        }

        InventoryTab placeableTab = CreateConveyorTab(inv);
        if(placeableTab != null)
        {
            activeTabs.Add(placeableTab);
            tabs[tabCounter].Activate(placeableTab);
            tabCounter++;
        }

        InventoryTab containerTab = CreateContainerTab(SaveMaster.UnlockedContainers());
        if(containerTab != null)
        {
            activeTabs.Add(containerTab);
            tabs[tabCounter].Activate(containerTab);
            tabCounter++;
        }

        InventoryTab stationTab = CreateStationTab(inv);
        if(stationTab != null)
        {
            activeTabs.Add(stationTab);
            tabs[tabCounter].Activate(stationTab);
            tabCounter++;
        }

        InventoryTab seatingTab = CreateSeatingTab(inv);
        if(seatingTab != null)
        {
            activeTabs.Add(seatingTab);
            tabs[tabCounter].Activate(seatingTab);
            tabCounter++;
        }

        InventoryTab decorTab = CreateDecorTab(inv);
        if(decorTab != null)
        {
            activeTabs.Add(decorTab);
            tabs[tabCounter].Activate(decorTab);
            tabCounter++;
        }


        RefreshInventory(activeTabs[0]);
    }

    /// <summary>For each slot in this Inventory, declares this InventoryMaster as its master. </summary>
    private void BecomeSlotMaster()
    {
        foreach(InventorySlot s in slots)
        {
            s.NewMaster(this);
        }
    }

    /// <summary>Sets this InventoryMaster's TileGrid.
    /// <br></br><em>Precondition:</em> <c>grid</c> is not <c>null</c>.</summary>
    public void SetGrid(TileGrid grid)
    {
        Assert.IsNotNull(grid, "Parameter grid cannot be null.");
        this.grid = grid;
    }

    /// <summary>Returns this InventoryMaster's TileGrid. </summary>
    public TileGrid MainGrid()
    {
        return grid;
    }

    /// <summary><strong>Returns:</strong> the created <c>InventoryTab</c> of <c>Edibles</c> from <c>fullInventory,</c>
    /// or <c>null</c> if none exist.</summary>
    private InventoryTab CreateEdibleTab(List<Item> fullInventory)
    {
        List<Item> edibleItems = new List<Item>();

        foreach(Item i in fullInventory)
        {
            if(i is Edible && !edibleItems.Contains(i))
            {
                edibleItems.Add(i);
            }
        }
        if (edibleItems.Count == 0) return null;
        else
        {
            InventoryTab tab = new InventoryTab("Foods", edibleItems);
            return tab;
        }
    }

    /// <summary><strong>Returns:</strong> the created <c>InventoryTab</c> of <c>IPlaceables</c> from <c>fullInventory,</c>
    /// or <c>null</c> if none exist.</summary>
    private InventoryTab CreateConveyorTab(List<Item> fullInventory)
    {
        List<Item> conveyorItems = new List<Item>();

        foreach (Item i in fullInventory)
        {
            if (i is DefaultConveyor && !conveyorItems.Contains(i))
            {
                conveyorItems.Add(i);
            }
        }
        if (conveyorItems.Count == 0) return null;
        else
        {
            InventoryTab tab = new InventoryTab("Conveyors", conveyorItems);
            return tab;
        }
    }

    /// <summary><strong>Returns:</strong> the created <c>InventoryTab</c> of unlocked <c>Containers</c>, or <c>null</c>
    /// if none are unlocked.</summary>
    private InventoryTab CreateContainerTab(List<Item> unlockedContainers)
    {
        if (unlockedContainers.Count == 0) return null;
        InventoryTab tab = new InventoryTab("Containers", unlockedContainers);
        return tab;
    }

    /// <summary><strong>Returns:</strong> the created <c>InventoryTab</c> of <c>StationInventories</c> from <c>fullInventory,</c>
    /// or <c>null</c> if none exist.</summary>
    private InventoryTab CreateStationTab(List<Item> fullInventory)
    {
        List<Item> stationItems = new List<Item>();
        foreach (Item i in fullInventory)
        {
            if (i is StationInventory && !stationItems.Contains(i))
            {
                stationItems.Add(i);
            }
        }
        if (stationItems.Count == 0) return null;
        else
        {
            InventoryTab tab = new InventoryTab("Stations", stationItems);
            return tab;
        }
    }

    /// <summary><strong>Returns:</strong> the created <c>InventoryTab</c> of <c>StationInventories</c> from <c>fullInventory,</c>
    /// or <c>null</c> if none exist.</summary>
    private InventoryTab CreateSeatingTab(List<Item> fullInventory)
    {
        List<Item> seatingItems = new List<Item>();
        foreach (Item i in fullInventory)
        {
            if (i is Seating && !seatingItems.Contains(i))
            {
                seatingItems.Add(i);
            }
        }
        if (seatingItems.Count == 0) return null;
        else
        {
            InventoryTab tab = new InventoryTab("Seating", seatingItems);
            return tab;
        }
    }

    /// <summary><strong>Returns:</strong> the created <c>InventoryTab</c> of <c>StationInventories</c> from <c>fullInventory,</c>
    /// or <c>null</c> if none exist.</summary>
    private InventoryTab CreateDecorTab(List<Item> fullInventory)
    {
        List<Item> decorItems = new List<Item>();
        foreach (Item i in fullInventory)
        {
            if (i is Decor && !decorItems.Contains(i))
            {
                decorItems.Add(i);
            }
        }
        if (decorItems.Count == 0) return null;
        else
        {
            InventoryTab tab = new InventoryTab("Decor", decorItems);
            return tab;
        }
    }




    /// <summary>Activates <c>tabToReferesh</c> in the inventory.</summary>
    public void RefreshInventory(InventoryTab tabToRefresh)
    {
        int counter = 0;
        foreach(Item i in tabToRefresh.Items())
        {
            slots[counter].FillSlot(i);
            counter++;
        }
    }



    
}


