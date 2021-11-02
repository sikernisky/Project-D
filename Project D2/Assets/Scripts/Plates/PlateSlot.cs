using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSlot : MonoBehaviour
{
    public int SlotNumber { get; set; }

    public string FoodHoldingName { get; private set; }

    private SpriteRenderer plateSlotSpriteRenderer;

    private void Start()
    {
        plateSlotSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsOpen()
    {
        if (plateSlotSpriteRenderer.sprite == null) return true; 
        return false;
    }

    public void AddFoodSpriteToSlot(Sprite foodSprite, string foodName)
    {
        plateSlotSpriteRenderer.sprite = foodSprite;
        FoodHoldingName = foodName;

    }

    
}
