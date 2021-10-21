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

    public List<int> OrderedFoodRemaining { get; private set; }

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
        OrderedFoodRemaining = new List<int>();

        int counter = 0;
        foreach (KeyValuePair<string,int> item in itemsToAssign)
        {
            FoodScriptable currentFoodScriptable = FoodGenerator.GetFoodScriptableObject(item.Key);
            if(counter < childSlots.Count) childSlots[counter].FillSlot(currentFoodScriptable, item.Value); // fill the slot
            OrderedInventoryItems.AddLast(currentFoodScriptable);
            OrderedFoodRemaining.Add(item.Value);
            counter++;
        }
        
    }

    /**Fills as many slots as possible from an existing OrderedInventoryItems. */
    private void AssignChildSlotsTheirItems(LinkedList<FoodScriptable> itemsToAssign, List<int> foodsRemaining)
    {
        int counter = 0;
        foreach (FoodScriptable item in itemsToAssign)
        {
            if (counter < childSlots.Count) childSlots[counter].FillSlot(item, foodsRemaining.ElementAt(counter)); // fill the slot
            counter++;
        }

    }

    public void MoveInventoryLeft()
    {
        if (OrderedInventoryItems.Count <= childSlots.Count) return;

        FoodScriptable firstElementFoodScriptable = OrderedInventoryItems.First.Value;
        OrderedInventoryItems.RemoveFirst();

        int firstElementFoodRemaining = OrderedFoodRemaining[0];
        OrderedFoodRemaining.RemoveAt(0);

        OrderedInventoryItems.AddLast(firstElementFoodScriptable);
        OrderedFoodRemaining.Insert(OrderedFoodRemaining.Count, firstElementFoodRemaining);

        AssignChildSlotsTheirItems(OrderedInventoryItems, OrderedFoodRemaining);

    }

    public void MoveInventoryRight()
    {
        if (OrderedInventoryItems.Count <= childSlots.Count) return;

        FoodScriptable lastElementFoodScriptable = OrderedInventoryItems.Last.Value;
        OrderedInventoryItems.AddFirst(lastElementFoodScriptable);

        int lastElementFoodRemaining = OrderedFoodRemaining[OrderedFoodRemaining.Count - 1];
        OrderedFoodRemaining.Insert(0,lastElementFoodRemaining);

        AssignChildSlotsTheirItems(OrderedInventoryItems, OrderedFoodRemaining);

        OrderedInventoryItems.RemoveLast();
        OrderedFoodRemaining.RemoveAt(OrderedFoodRemaining.Count -1);
    }


    /** Called by an InventorySlot to get the Food it holds to decrement its remaining items by1 . */
    public int DecrementFood(FoodScriptable foodToDecrement, int byAmount = 1)
    {
        int i = 0;
        foreach(FoodScriptable item in OrderedInventoryItems)
        {
            if (item != foodToDecrement) i++;
            else break;
        }

        if (OrderedFoodRemaining[i] - byAmount < 0) OrderedFoodRemaining[i] = 0;
        else OrderedFoodRemaining[i] = OrderedFoodRemaining[i] - byAmount;
        return OrderedFoodRemaining[i];
    }

}
