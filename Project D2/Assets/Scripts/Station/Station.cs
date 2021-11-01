using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Station : Item, IAnimator
{

    /**Name of this Station. Corresponds to its Class name. */
    public override string NAME { get; } = "Station";

    /** A list of Foods this Station can interact with, or:
     * A list of one element with value "All", indicating it can take any food.*/
    public virtual string[] ItemsCanTake { get; } = { "All" };

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


    protected virtual void Start()
    {
        Scriptable = (StationScriptable)ItemGenerator.GetScriptableObject(NAME);
        SetUpAnimationTracks();
        SetUpHolders();

    }


    /** Returns true of this Station can process the item. */
    public bool CanProcess(string itemName)
    {
        if (ItemsCanTake[0] == "All" || Array.IndexOf(ItemsCanTake, itemName) >= 0) return true;
        return false;
    }

    /**Sets up all necessary holders for this station. */
    public virtual void SetUpHolders()
    {
    }

    /**DEFINE ME.*/
    public override void TakeItem(GameObject item)
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector2(0, 0);

        HoldItem(item);
    }

    public virtual void HoldItem(GameObject item)
    {
        ProcessItem(item);
    }

    /** Places its first item in the most available Holder, holds it, then passes it to the 
 *  next appropriate GameTile.*/
    public virtual void ProcessItem(GameObject item)
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
            DestroyItem(item);
            yield break;
        }

        Vector3 targetDestination = GameTileIn.NextGameTile.objectHolding.transform.position;
        Vector3 distanceToTravel = targetDestination - item.transform.position;
        Vector3 distancePerTick = distanceToTravel / NUM_MOVEMENT_TICKS;
        float secondsBetweenTick = .5f / NUM_MOVEMENT_TICKS;

        for (int i = 0; i < NUM_MOVEMENT_TICKS; i++)
        {
            item.transform.position += distancePerTick;
            yield return new WaitForSeconds(secondsBetweenTick);
        }

        if (GameTileIn.NextGameTile.objectHolding == null) DestroyItem(item);
        else GiveItem(item, GameTileIn.NextGameTile.objectHolding.GetComponent<Item>());
    }

    /**Destroys an item held in one of this Station's Holders.*/
    public override void DestroyItem(GameObject item)
    {
        Destroy(item);
    }

    /**Cashes an item in.*/
    public override void CashItemIn(GameObject item)
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
