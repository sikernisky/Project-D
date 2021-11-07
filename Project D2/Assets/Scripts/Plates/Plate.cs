using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public List<PlateSlot> Slots { get; private set; }

    public SpriteRenderer PlateSpriteRenderer { get; private set; }


    private void Start()
    {

        PlateSpriteRenderer = GetComponent<SpriteRenderer>();
        GatherPlateSlots();

    }

    /**Adds a Food to this plate. Precondition: foodToAdd must correspond to a Food class. */
    public void AddFoodToPlate(FoodScriptable foodToAdd)
    {
        if (GetNextOpenSlot() == null)
        {
            Debug.Log("No available slots on this plate.");
            return;
        }
        else{
            gameObject.AddComponent(ItemGenerator.GetClassFromString(foodToAdd.itemClassName));
            GetNextOpenSlot().AddFoodSpriteToSlot(foodToAdd.plateSprite, foodToAdd.itemClassName);
        }
    }

    /**Adds a Food to this plate. Precondition: foodToAdd must correspond to a Food class. */
    public void AddFoodToPlate(FoodScriptable foodToAdd, Sprite foodSprite)
    {
        if (GetNextOpenSlot() == null)
        {
            Debug.Log("No available slots on this plate.");
        }
        else
        {
            GetNextOpenSlot().AddFoodSpriteToSlot(foodSprite, foodToAdd.itemClassName);
        }
    }

    private PlateSlot GetNextOpenSlot()
    {
        foreach(PlateSlot plateSlot in Slots)
        {
            if (plateSlot.IsOpen()) return plateSlot;
        }
        return null;
    }


    private void GatherPlateSlots()
    {
        Slots = new List<PlateSlot>();

        int counter = 0;
        foreach(Transform child in transform)
        {
            if(child.GetComponent<PlateSlot>() != null)
            {
                PlateSlot childSlotComponent = child.GetComponent<PlateSlot>();
                childSlotComponent.SlotNumber = counter;
                Slots.Add(childSlotComponent);
                counter++;
            }
        }
    }
    

    

    public override string ToString()
    {
        return null;
    }


}
