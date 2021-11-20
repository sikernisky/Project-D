using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    /**The slots on this plate. */
    public List<PlateSlot> Slots { get; private set; }

    /**The SpriteRenderer of this plate. */
    public SpriteRenderer PlateSpriteRenderer { get; private set; }

    /**The value of this plate. */
    private float plateValue;

    public float PlateValue
    {
        get { return plateValue; }
        set
        {
            if(value >= 0)
            {
                plateValue = value;
            }
        }
    }


    private void Start()
    {

        PlateSpriteRenderer = GetComponent<SpriteRenderer>();
        GatherPlateSlots();

    }


    /**Adds a Food to this plate. Precondition: foodToAdd must correspond to a Food class. */
    public PlateSlot AddFoodToPlate(Food foodToAdd, Sprite foodSprite)
    {
        return SetFoodToSlot(foodToAdd, foodSprite);
    }

    private PlateSlot SetFoodToSlot(Food foodToAdd, Sprite foodSprite)
    {
        if (GetNextOpenSlot() == null) return null;

        PlateSlot plateSlot = GetNextOpenSlot();
        plateSlot.AddFoodToSlot(foodSprite, foodToAdd);
        return plateSlot;
    }

    private void UpdatePlateValue(Food food)
    {
        PlateValue += food.BASE_REWARD;
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
                childSlotComponent.ParentPlate = this;
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
