using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Station : FluidItem, IAnimator
{

    /**Name of this Station. Corresponds to its Class name. */
    public override string NAME { get; } = "Station";

    /** All items that can be dragged and dropped onto/into this Item.
     *  If any item can be dragged to this item, it contains one element "All"
     *  If no item can be dragged to this item, it contains no elements or is null. */
    public override string[] ItemsCanTakeByDragging { get; } = { "All" };

    /** All items that can be moved onto/into this Item from an IMover.
     *  If any item can be moved to this item, it contains one element "All"
     *  If no item can be moved to this item, it contains no elements or is null. */
    public override string[] ItemsCanTakeByMovement { get; } = { "All" };

    /** The amount of time, in seconds, it takes to hold an item. */
    public virtual float TimeToHold { get; set; }

    /** An array of ALL StationHolders for this Station. */
    public virtual StationHolder[] Holders { get; protected set; }

    /**The number of times a GameObject on this Station moves closer to its target.*/
    public const int NUM_MOVEMENT_TICKS = 6;

    /**Accepts an item dragged onto this station from the Inventory.*/
    public override bool TakeDraggedItem(ItemScriptable item)
    {
        if (!CanDragTo(item)) return false;
        else Debug.Log(NAME + " accepted " + item.itemClassName);
        return true;
    }

    public override void ReleaseDraggedItem()
    {
        throw new NotImplementedException();
    }

    /**Get the scriptable object of this station. Set up animations and holders.*/
    protected override void Start()
    {
        base.Start();
    }

    /**Claims an item and performs any necessary actions before HoldMovedItem() is called.*/
    public override void TakeMovedItem(GameObject item)
    {
        HoldMovedItem(item);
    }

    /**Calls ProcessMovedItem. Child classes may override to perform necessary actions.*/
    public virtual void HoldMovedItem(GameObject item)
    {
        ProcessMovedItem(item);
    }

    /** Places its first item in the most available Holder, holds it, then passes it to the 
 *  next appropriate GameTile.*/
    public virtual void ProcessMovedItem(GameObject item)
    {
        MoveItem(item);
        TargetPosition = GetTargetDestination();
    }

    public virtual bool MoveToHolder(GameObject item)
    {
        return true;
    }


    /**Moves this item from a Holder to the next appropriate GameTile's transform.position.*/
    public override void MoveItem(GameObject item)
    {
        if (!CanContinue(this)) DestroyMovedItem(item);
        StartCoroutine(MoveItemCoro(item, GameTileIn.NextGameTile.objectHolding.transform.position));
    }

    /**Moves this item from a Holder to targetDestination.*/
    public override void MoveItem(GameObject item, Vector3 targetDestination)
    {
        if (!CanContinue(this)) DestroyMovedItem(item);
        StartCoroutine(MoveItemCoro(item, targetDestination));
    }

    /**Move item to targetDestination and call GiveMovedItem(). */
    IEnumerator MoveItemCoro(GameObject item, Vector3 targetDestination)
    {
        if(GameTileIn.NextGameTile.objectHolding == null)
        {
            DestroyMovedItem(item);
            yield break;
        }

        Vector3 distanceToTravel = targetDestination - item.gameObject.transform.position;
        Vector3 distancePerTick = distanceToTravel / NUM_MOVEMENT_TICKS;
        float secondsBetweenTick = .5f / NUM_MOVEMENT_TICKS;

        for (int i = 0; i < NUM_MOVEMENT_TICKS; i++)
        {
            item.transform.position += distancePerTick;
            yield return new WaitForSeconds(secondsBetweenTick);
        }

        if (GameTileIn.NextGameTile.objectHolding == null) DestroyMovedItem(item);
        else GiveMovedItem(item, GameTileIn.NextGameTile.objectHolding.GetComponent<FluidItem>());
    }

    /**Destroys an item held in one of this Station's Holders.*/
    public override void DestroyMovedItem(GameObject item)
    {
        if (item.GetComponent<Plate>() != null)
        {
            LevelData.levelEmBalance += item.GetComponent<Plate>().PlateValue;
        }
        Destroy(item);
    }


    protected virtual void HideHeldItem(GameObject item)
    {
        item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        foreach (Transform itemChild in item.transform)
        {
            if (itemChild.GetComponent<SpriteRenderer>() != null) itemChild.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        }
    }

    protected virtual void ShowHeldItem(GameObject item)
    {
        item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        foreach (Transform itemChild in item.transform)
        {
            if (itemChild.GetComponent<SpriteRenderer>() != null) itemChild.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }



    /**Cashes an item in.*/
    public override void CashMovedItemIn(GameObject item)
    {

    }

    public virtual Coroutine PlayAnimation(Sprite[] animationTrackToPlay, float secondsBetween, SpriteRenderer rendererToAnimate)
    {
        return StartCoroutine(PlayAnimationCoro(animationTrackToPlay, secondsBetween, rendererToAnimate));
    }

    IEnumerator PlayAnimationCoro(Sprite[] animationTrackToPlay, float secondsBetween, SpriteRenderer rendererToAnimate)
    {
        while (true)
        {
            foreach (Sprite sprite in animationTrackToPlay)
            {
                rendererToAnimate.sprite = sprite;
                yield return new WaitForSeconds(secondsBetween);
            }
        }
    }


    public void StopAnimation(Coroutine animationToStop)
    {
        this.StopCoroutine(animationToStop);
    }

    public virtual void PutHeldItemOnPlate(Plate plateToPutOn)
    {

    }

}
