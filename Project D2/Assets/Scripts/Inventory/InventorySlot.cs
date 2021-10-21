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
       null if the slot is empty. Set in Inspector.*/
    public Image draggableImage;

    /**A sprite for when the slot is empty.*/
    public Sprite emptySlotSprite;

    /**A sprite for the slot background */
    public Sprite defaultSlotSprite;

    /**A TMP_Text controlled by this slot. Details the amount remaining of the item it holds. */
    public TMP_Text numRemainingText;

    /**True if this slot holds no food item, false otherwise. */
    public bool IsEmpty { get; private set; }

    /**True if this slot is out of food, false otherwise. */
    public bool OutOfFood { get; private set; }

    /**Number of foods remaining in this slot. */
    public int FoodRemaining { get; set; }

    /**The position this Food's GameObject returns to OnDrop. */
    public Vector3 ReturnPosition { get; private set; }

    private void Start()
    {
        slotImage = GetComponent<Image>();
        if (FoodHolding == null) EmptySlot();
        ReturnPosition = draggableImage.transform.position;
    }

    private void Update()
    {
        MaintainFoodStock();
    }

    /**Empties the slot completely, removing the food inside.*/
    public void EmptySlot()
    {
        FoodHolding = null;
        slotImage.sprite = emptySlotSprite;
        draggableImage.gameObject.SetActive(false);
        FoodRemaining = 0;
        IsEmpty = true;
    }

    /**Updates the slot depending on if there is food remaining.*/
    public void MaintainFoodStock()
    {
        //!!!! Implement these two lines below when ready.
        //if (FoodRemaining > 0 && OutOfFood) UpdateSlotWithStockedFood();
        //else if (FoodRemaining < 1 && !OutOfFood) UpdateSlotWithEmptyFood();\

        numRemainingText.text = FoodRemaining.ToString();
    }

    /**Fills the slot. */
    public void FillSlot(FoodScriptable foodToSet, int numRemaining = 0)
    {
        if (slotImage == null) slotImage = GetComponent<Image>();
        FoodHolding = foodToSet;
        FoodRemaining = numRemaining;
        draggableImage.gameObject.SetActive(true);
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
        draggableImage.sprite = FoodHolding.greyDraggableSprite;
    }


    /**Drags this draggable Image sprite. */
    public void OnDrag(PointerEventData eventData)
    {
        draggableImage.gameObject.transform.position = Input.mousePosition;
        slotImage.sprite = defaultSlotSprite;
    }

    /**Drops this draggable Image sprite. */
    public void OnDrop(PointerEventData eventData)
    {
        draggableImage.gameObject.transform.position = ReturnPosition;
        slotImage.sprite = FoodHolding.slotBackground;

        //DELETE ME!!
        FoodRemaining--;
    }
}
