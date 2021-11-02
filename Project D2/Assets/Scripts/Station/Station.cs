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


    /**A list of this Station's Holders. Null if it has none. */
    public StationHolder[] Holders { get; protected set; }

    /**An array of sprites, in order, that form this Station's Holder animation. */
    public Sprite[] HolderAnimationTrack { get; protected set; }

    /**This Station's StationScriptable counterpart.*/
    public StationScriptable Scriptable { get; protected set; }

    /** The amount of time, in seconds, it takes to hold an item. */
    public virtual float TimeToProcess { get; }

    /**The number of times a GameObject on this Station moves closer to its target.*/
    public const int NUM_MOVEMENT_TICKS = 6;


    public override void TakeDraggedItem(ItemScriptable item)
    {
        return;
    }

    protected virtual void Start()
    {
        Scriptable = (StationScriptable)ItemGenerator.GetScriptableObject(NAME);
        SetUpAnimationTracks();
        SetUpHolders();

    }



    /**Sets up all necessary holders for this station. */
    public virtual void SetUpHolders()
    {
    }

    /**DEFINE ME.*/
    public override void TakeMovedItem(GameObject item)
    {
        HoldMovedItem(item);
    }

    public virtual void HoldMovedItem(GameObject item)
    {
        ProcessMovedItem(item);
    }

    /** Places its first item in the most available Holder, holds it, then passes it to the 
 *  next appropriate GameTile.*/
    public virtual void ProcessMovedItem(GameObject item)
    {
        MoveItem(item);
    }


    /**Moves this item from a Holder to the next appropriate GameTile.*/
    public override void MoveItem(GameObject item)
    {
        StartCoroutine(MoveItemCoro(item));
    }


    IEnumerator MoveItemCoro(GameObject item)
    {
        yield return new WaitForSeconds(TimeToProcess);

        if(GameTileIn.NextGameTile.objectHolding == null)
        {
            DestroyMovedItem(item);
            yield break;
        }

        Vector3 targetDestination = GameTileIn.NextGameTile.objectHolding.transform.position;
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
        Destroy(item);
    }

    /**Cashes an item in.*/
    public override void CashMovedItemIn(GameObject item)
    {

    }

    public virtual Coroutine PlayAnimation(Sprite[] animationTrackToPlay, float secondsBetween, SpriteRenderer rendererToAnimate)
    {
        Coroutine coroToReturn = null;

        if (animationTrackToPlay == HolderAnimationTrack)
        {
            coroToReturn = StartCoroutine(PlayAnimationCoro(animationTrackToPlay, secondsBetween, rendererToAnimate));
        }
        return coroToReturn;
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

    public virtual void StopAnimation(Coroutine animationToStop)
    {
        throw new NotImplementedException();
    }

    public virtual void SetUpAnimationTracks()
    {
        HolderAnimationTrack = Scriptable.holderReadyAnimationTrack;
    }
}
