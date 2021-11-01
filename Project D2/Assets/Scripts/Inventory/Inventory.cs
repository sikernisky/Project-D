using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /**A list of all slots in this inventory. Set in Start(). */
    public List<InventorySlot> childSlots { get; private set; }

    /**The GameObject that holds all slots in the Hierarchy. Set in Start(). */
    private GameObject slotMaster;

    public static FoodScriptable FoodDragging { get; set; }

    public LinkedList<ItemScriptable> OrderedInventoryItems { get; private set; }

    public List<int> OrderedItemRemaining { get; private set; }

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
        OrderedInventoryItems = new LinkedList<ItemScriptable>();
        OrderedItemRemaining = new List<int>();

        int counter = 0;
        foreach (KeyValuePair<string,int> item in itemsToAssign)
        {
            ItemScriptable currentScriptable = ItemGenerator.GetScriptableObject(item.Key);
            if(counter < childSlots.Count) childSlots[counter].FillSlot(currentScriptable, item.Value); // fill the slot
            OrderedInventoryItems.AddLast(currentScriptable);
            OrderedItemRemaining.Add(item.Value);
            counter++;
        }
        
    }

    /**Fills as many slots as possible from an existing OrderedInventoryItems. */
    private void AssignChildSlotsTheirItems(LinkedList<ItemScriptable> itemsToAssign, List<int> itemsRemaining)
    {
        int counter = 0;
        foreach (ItemScriptable item in itemsToAssign)
        {
            if (counter < childSlots.Count) childSlots[counter].FillSlot(item, itemsRemaining.ElementAt(counter)); // fill the slot
            counter++;
        }

    }

    public void MoveInventoryLeft()
    {
        if (OrderedInventoryItems.Count <= childSlots.Count) return;

        ItemScriptable firstElementScriptable = OrderedInventoryItems.First.Value;
        OrderedInventoryItems.RemoveFirst();

        int firstElementRemaining = OrderedItemRemaining[0];
        OrderedItemRemaining.RemoveAt(0);

        OrderedInventoryItems.AddLast(firstElementScriptable);
        OrderedItemRemaining.Insert(OrderedItemRemaining.Count, firstElementRemaining);

        AssignChildSlotsTheirItems(OrderedInventoryItems, OrderedItemRemaining);

    }

    public void MoveInventoryRight()
    {
        if (OrderedInventoryItems.Count <= childSlots.Count) return;

        ItemScriptable lastElementScriptable = OrderedInventoryItems.Last.Value;
        OrderedInventoryItems.AddFirst(lastElementScriptable);

        int lastElementRemaining = OrderedItemRemaining[OrderedItemRemaining.Count - 1];
        OrderedItemRemaining.Insert(0, lastElementRemaining);

        AssignChildSlotsTheirItems(OrderedInventoryItems, OrderedItemRemaining);

        OrderedInventoryItems.RemoveLast();
        OrderedItemRemaining.RemoveAt(OrderedItemRemaining.Count -1);
    }


    /** Called by an InventorySlot to get the Item it holds to decrement its remaining items by1 . */
    public int DecrementItem(ItemScriptable itemToDecrement, int byAmount = 1)
    {
        int i = 0;
        foreach(ItemScriptable item in OrderedInventoryItems)
        {
            if (item != itemToDecrement) i++;
            else break;
        }

        if (OrderedItemRemaining[i] - byAmount < 0) OrderedItemRemaining[i] = 0;
        else OrderedItemRemaining[i] = OrderedItemRemaining[i] - byAmount;
        return OrderedItemRemaining[i];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = true;
    }
}
