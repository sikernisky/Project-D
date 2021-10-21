using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    /**A list of all slots in this inventory. Set in Start(). */
    public List<InventorySlot> childSlots { get; private set; }

    /**The GameObject that holds all slots in the Hierarchy. Set in Start(). */
    private GameObject slotMaster;

    public LinkedList<FoodScriptable> OrderedInventoryItems { get; private set; }

    public LinkedList<int> OrderedFoodRemaining { get; private set; }

    void Start()
    {
        slotMaster = GameObject.Find("SlotMaster");

        childSlots = GatherChildSlots();
        AssignChildSlotsTheirNumbers();
        SaveMaster.saveData.itemsToLoadInInventory.Add("Broccoli", 10);
        SaveMaster.saveData.itemsToLoadInInventory.Add("GroundBeef", 2);
        SaveMaster.saveData.itemsToLoadInInventory.Add("SlicedStrawberry", 4);
        SaveMaster.saveData.itemsToLoadInInventory.Add("BokChoy", 3);
        SaveMaster.saveData.itemsToLoadInInventory.Add("MarinatedCarnitas",9);
        SaveMaster.saveData.itemsToLoadInInventory.Add("BananaChunks", 1);
        SaveMaster.saveData.itemsToLoadInInventory.Add("RoastDuck", 8);
        AssignChildSlotsTheirItems(SaveMaster.saveData.itemsToLoadInInventory);
    }

    /**Returns a list of InventorySlots based on their position in the Hierarchy. */
    private List<InventorySlot> GatherChildSlots()
    {
        List<InventorySlot> output = new List<InventorySlot>();

        foreach(Transform child in slotMaster.transform)
        {
            if(child.gameObject.GetComponent<InventorySlot>() != null)
            {
                output.Add(child.GetComponent<InventorySlot>());
            }
        }

        return output;
    }

    /**Assigns each InventorySlot a number based on their position in the Hierarchy. */
    private void AssignChildSlotsTheirNumbers()
    {
        int counter = 1;
        foreach(InventorySlot slot in childSlots)
        {
            slot.SlotNumber = counter;
            counter++;
        }
    }


    /**Sets up OrderedInventoryItems and fills as many slots as possible. */
    private void AssignChildSlotsTheirItems(Dictionary<string, int> itemsToAssign)
    {
        OrderedInventoryItems = new LinkedList<FoodScriptable>();
        OrderedFoodRemaining = new LinkedList<int>();

        int counter = 0;
        foreach (KeyValuePair<string,int> item in itemsToAssign)
        {
            FoodScriptable currentFoodScriptable = FoodGenerator.GetFoodScriptableObject(item.Key);
            if(counter < childSlots.Count) childSlots[counter].FillSlot(currentFoodScriptable, item.Value); // fill the slot
            OrderedInventoryItems.AddLast(currentFoodScriptable);
            OrderedFoodRemaining.AddLast(item.Value);
            counter++;
        }
        
    }

    /**Fills as many slots as possible from an existing OrderedInventoryItems. */
    private void AssignChildSlotsTheirItems(LinkedList<FoodScriptable> itemsToAssign, LinkedList<int> foodsRemaining)
    {
        int counter = 0;
        foreach (FoodScriptable item in itemsToAssign)
        {
            if (counter < childSlots.Count) childSlots[counter].FillSlot(item, foodsRemaining.ElementAt(counter)); // fill the slot
            Debug.Log("Trying ElementAt at counter value " + counter.ToString());
            Debug.Log(foodsRemaining.ElementAt(counter));
            counter++;
        }

    }

    public void MoveInventoryLeft()
    {
        if (OrderedInventoryItems.Count <= childSlots.Count) return;

        FoodScriptable firstElement = OrderedInventoryItems.First.Value;
        OrderedInventoryItems.RemoveFirst();
        AssignChildSlotsTheirItems(OrderedInventoryItems, OrderedFoodRemaining);
        OrderedInventoryItems.AddLast(firstElement);


    }

    public void MoveInventoryRight()
    {
        if (OrderedInventoryItems.Count <= childSlots.Count) return;

        FoodScriptable lastElement = OrderedInventoryItems.Last.Value;
        OrderedInventoryItems.AddFirst(lastElement);
        AssignChildSlotsTheirItems(OrderedInventoryItems, OrderedFoodRemaining);
        OrderedInventoryItems.RemoveLast();

    }
}
