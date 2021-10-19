using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /**A list of all slots in this inventory. Set in Start(). */
    public List<InventorySlot> childSlots { get; private set; }

    /**The GameObject that holds all slots in the Hierarchy. Set in Start(). */
    private GameObject slotMaster;

    void Start()
    {
        slotMaster = GameObject.Find("SlotMaster");

        childSlots = GatherChildSlots();
        AssignChildSlotsTheirNumbers();
        SaveMaster.saveData.itemsToLoadInInventory.Add("Broccoli");
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


    /**Calls FillSlot on each child, in ascending order.
     
        Example: if itemsToAssign = "Broccoli", "Beef", "Carrot"
                 slot1 gets Broccoli, slot2 gets Beef.. etc. */
    private void AssignChildSlotsTheirItems(List<string> itemsToAssign)
    {
        if (itemsToAssign.Count > childSlots.Count) Debug.Log("# of items can't be greater than # of slots.");
        else
        {
            int counter = 0;
            foreach (string item in itemsToAssign)
            {
                childSlots[counter].FillSlot(FoodGenerator.GetFoodScriptableObject(item));
                counter++;
            }
        }

    }
}
