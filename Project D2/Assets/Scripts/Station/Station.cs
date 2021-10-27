using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Station : Item, IMover, IAnimator
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


    public void Start()
    {
        Scriptable = (StationScriptable)ItemGenerator.GetScriptableObject(NAME);
        SetUpAnimationTracks();
        SetUpHolders();

        foreach(StationHolder holder in Holders)
        {
            holder.HolderAnimation = PlayAnimation(HolderAnimationTrack, .2f, holder.HolderSpriteRenderer);
        }

    }

    /** Places its first item in the most available Holder, holds it, then passes it to the 
     *  next appropriate GameTile.*/
    public virtual void ProcessItem(GameObject itemToProcess, string itemName)
    {

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
    public virtual void TakeItem(GameObject item)
    {

    }

    /**Moves this item from a Holder to the next appropriate GameTile.*/
    public virtual void MoveItem(GameObject item)
    {

    }

    /**Gives an Item it holds to another IMover.*/
    public virtual void GiveItem(GameObject item)
    {

    }

    /**Destroys an item held in one of this Station's Holders.*/
    public virtual void DestroyItem(GameObject item)
    {

    }

    /**Cashes an item in.*/
    public virtual void CashItemIn(GameObject item)
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
