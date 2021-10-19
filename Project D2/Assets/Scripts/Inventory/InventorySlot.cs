using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IDragHandler, IDropHandler
{
    /**The food this slot is holding. */
    public FoodScriptable FoodHolding { get; set; }
    /**The number of this slot, dictated by the Inventory class. */
    public int SlotNumber { get; set; }

    /**The image component of this slot. */
    private Image slotImage;

    /**The image component of a child GameObject which holds an item without a background. 
       null if the slot is empty.*/
    public Image draggableImage;

    /**A sprite for when the slot is empty.*/
    public Sprite emptySlotSprite;

    /**A sprite for the slot background */
    public Sprite defaultSlotSprite;

    /**A TMP_Text controlled by this slot. Details the name of the item it holds. */
    public TMP_Text itemNameText;

    /**A TMP_Text controlled by this slot. Details the amount remaining of the item it holds. */
    public TMP_Text numRemainingText;

    /**True if this slot holds no food item, false otherwise. */
    public bool IsEmpty { get; private set; }

    /**True if this slot is out of food, false otherwise. */
    public bool OutOfFood { get; private set; }

    /**Number of foods remaining in this slot. */
    public int foodRemaining { get; set; }

    private void Start()
    {
        slotImage = GetComponent<Image>();
        if (FoodHolding == null) EmptySlot();
    }

    private void Update()
    {
        //MaintainFoodStock();
    }

    /**Empties the slot completely, removing the food inside.*/
    public void EmptySlot()
    {
        FoodHolding = null;
        slotImage.sprite = emptySlotSprite;
        draggableImage.gameObject.SetActive(false);
        foodRemaining = 0;
        IsEmpty = true;
    }

    /**Updates the slot depending on if there is food remaining.*/
    public void MaintainFoodStock()
    {
        if (foodRemaining > 0 && OutOfFood) UpdateSlotWithStockedFood();
        else if (foodRemaining < 1 && !OutOfFood) UpdateSlotWithEmptyFood();
    }

    /**Fills the slot. */
    public void FillSlot(FoodScriptable foodToSet)
    {
        if (slotImage == null) slotImage = GetComponent<Image>();
        FoodHolding = foodToSet;
        slotImage.sprite = FoodHolding.slotBackground;
        UpdateSlotWithStockedFood();
        IsEmpty = false;
    }

    /**Updates the slot to show the item, ready to drag. */
    private void UpdateSlotWithStockedFood()
    {
        draggableImage.sprite = FoodHolding.draggableSprite;
        OutOfFood = false;
    }

    /**Updates the slot to show the item greyed out and unable to drag. */
    private void UpdateSlotWithEmptyFood()
    {
        OutOfFood = true;
    }


    /**Drags this draggable Image sprite. */
    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    /**Drops this draggable Image sprite. */
    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
