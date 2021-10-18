using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    /**The food this slot is holding. */
    public FoodScriptable FoodHolding;

    /**The number of this slot, dictated by the Inventory class. */
    public int SlotNumber { get; set; }

    /**The image component of this slot. */
    private Image slotImage;

    /**A sprite for when the slot is empty.*/
    public Sprite greySlotSprite;

    /**A sprite for when the slot is empty, but in rarer cases.*/
    public Sprite normalSlotSprite;

    /**A TMP_Text controlled by this slot. Details the name of the item it holds. */
    public TMP_Text itemNameText;

    /**A TMP_Text controlled by this slot. Details the amount remaining of the item it holds. */
    public TMP_Text numRemainingText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**Sets the slotImage field based on FoodHolding.
       If FoodHolding is null, defaults to grey.*/
    private void SetSlotImage()
    {
        if (FoodHolding == null) slotImage.sprite = greySlotSprite;
        else slotImage.sprite = FoodHolding.inventorySprite;
    }
}
