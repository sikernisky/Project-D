using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IDragHandler, IDropHandler
{
    /**The Item this slot is holding. */
    public ItemScriptable ItemHolding { get; set; }

    /**The number of this slot, dictated by the Inventory class. */
    public int SlotNumber { get; set; }

    /**The image component of this slot. */
    private Image slotImage;

    /**The image component of a child GameObject which holds an Item without a background. 
       null if the slot is empty. Set in Inspector.*/
    public Image draggableImage;

    /**A sprite for when the slot is empty.*/
    public Sprite emptySlotSprite;

    /**A sprite for the slot background */
    public Sprite defaultSlotSprite;

    /**The Inventory that holds this slot. */
    public Inventory parentInventory;

    /**A TMP_Text controlled by this slot. Details the amount remaining of the Item it holds. */
    public TMP_Text numRemainingText;

    /**True if this slot holds no Item, false otherwise. */
    public bool IsEmpty { get; private set; }

    /**True if this slot is out of Item, false otherwise. */
    public bool OutOfItem { get; private set; }

    /**Amount of Item remaining in this slot. */
    public int ItemRemaining { get; set; }

    /**The position this Item's GameObject returns to OnDrop. */
    public Vector3 ReturnPosition { get; private set; }

    private void Start()
    {
        slotImage = GetComponent<Image>();
        if (ItemHolding == null) EmptySlot();
        ReturnPosition = draggableImage.gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        MaintainItemStock();
    }

    /**Empties the slot completely, removing the Item inside.*/
    public void EmptySlot()
    {
        ItemHolding = null;
        slotImage.sprite = emptySlotSprite;
        draggableImage.gameObject.SetActive(false);
        IsEmpty = true;
    }

    /**Updates the slot depending on if there is Item remaining.*/
    public void MaintainItemStock()
    {
        //!!!! Implement these two lines below when ready.
        //if (ItemRemaining > 0 && OutOfItem) UpdateSlotWithStockedItem();
        //else if (ItemRemaining < 1 && !OutOfItem) UpdateSlotWithEmptyItem();\

        numRemainingText.text = ItemRemaining.ToString();
    }

    /**Fills the slot. */
    public void FillSlot(ItemScriptable itemToSet, int numRemaining = 0)
    {
        if (slotImage == null) slotImage = GetComponent<Image>();
        ItemHolding = itemToSet;
        ItemRemaining = numRemaining;
        draggableImage.gameObject.SetActive(true);
        slotImage.sprite = itemToSet.slotBackground;
        UpdateSlotWithStockedItem();
        IsEmpty = false;
    }



    /**Updates the slot to show the item, ready to drag. */
    private void UpdateSlotWithStockedItem()
    {
        draggableImage.sprite = ItemHolding.draggableSprite;
        OutOfItem = false;
    }

    /**Updates the slot to show the item greyed out and unable to drag. */
    private void UpdateSlotWithEmptyItem()
    {
        OutOfItem = true;
        draggableImage.sprite = ItemHolding.greyDraggableSprite;
    }


    /**Drags this draggable Image sprite. */
    public void OnDrag(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = false;
        draggableImage.gameObject.transform.position = Input.mousePosition;
        slotImage.sprite = defaultSlotSprite;
    }

    /**Drops this draggable Image sprite. */
    public void OnDrop(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = true;
        draggableImage.gameObject.GetComponent<RectTransform>().anchoredPosition = ReturnPosition;
        slotImage.sprite = ItemHolding.slotBackground;

        //DELETE ME!!
        ItemRemaining = parentInventory.DecrementItem(ItemHolding);
    }




}
