using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StationHolder : MonoBehaviour
{
    /**
     * TERMINOLOGY:
     *     Queued -> An item dragged into the holder, waiting to affect a moved item.
     *     Holding -> An item moved into the holder that will interact with the queued item.
     */


    /**This StationHolder's station. */
    public Station ParentStation;

    /**The SpriteRenderer of this StationHolder. */
    public SpriteRenderer HolderSpriteRenderer;

    /**The GameObject of this StationHolder. */
    public GameObject HolderGameObject;

    /**True if this StationHolder is holding a GameObject; false otherwise. */
    public bool Occupied { get; private set; }

    /**True if this StationHolder has a queued Item; false otherwise. */
    public bool Queued { get; set; }

    /**This StationHolder's transform.localPosition. */
    public Vector2 HolderPosition { get; private set; }

    /**The Scriptable of the Item this StationHolder has queued. */
    public ItemScriptable ItemQueuedScriptable { get; private set; }

    /**The Item this StationHolder has queued. */
    public Item ItemQueued { get; private set; }

    /**The GameObject this StationHolder is holding. */
    public GameObject ItemHolding { get; set; }

    /** All items that can be dragged and dropped onto/into this Item. */
    public string[] ItemsCanTakeByDragging { get; set; }

    /** All items that can be moved onto/into this Item from an IMover. */
    public string[] ItemsCanTakeByMovement { get; set; }


    public void HoldItem(GameObject itemToHold)
    {
        ItemHolding = itemToHold;
        Occupied = true;
    }

    public void QueueItem(ItemScriptable itemToQueueScriptable, Sprite newHolderSprite = null)
    {

        ItemQueuedScriptable = itemToQueueScriptable;
        ItemQueued = ItemGenerator.GetItemFromString(itemToQueueScriptable.itemClassName);

        Queued = true;
        if (newHolderSprite != null) HolderSpriteRenderer.sprite = newHolderSprite;

    }


    public void ReleaseHeldItem()
    {
        ItemHolding = null;
        Occupied = false;
    }

    public void ReleaseQueuedItem()
    {
        ItemQueuedScriptable = null;
        ItemQueued = null;
        Queued = false;
    }

    public void ApplyQueuedItemToHeldItem()
    {
        ItemQueued.CollideInteraction(ItemHolding.GetComponent<Item>());
        ReleaseQueuedItem();
        ReleaseHeldItem();
    }

    private void Start()
    {
        HolderPosition = transform.localPosition;
    }





}
