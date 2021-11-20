using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSlot : MonoBehaviour
{
    public int SlotNumber { get; set; }

    public Food FoodHolding { get; private set; }

    private SpriteRenderer plateSlotSpriteRenderer;

    public Plate ParentPlate { get; set; }

    private void Start()
    {
        plateSlotSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsOpen()
    {
        if (plateSlotSpriteRenderer.sprite == null) return true; 
        return false;
    }

    /** Adds a Food to this slot. Precondition: foodName must correspond to a Food. */
    public void AddFoodToSlot(Sprite foodSprite, Food food)
    {
        plateSlotSpriteRenderer.sprite = foodSprite;
        ParentPlate.PlateValue += food.CalculateReward();
        FoodHolding = food;
    }

    
}
