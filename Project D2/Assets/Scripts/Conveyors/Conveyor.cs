using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : FluidItem, IAnimator
{
    public enum Direction {North, East, South, West}

    /**Name of this conveyor. Corresponds to its Class name. */
    public override string NAME { get; } = "Conveyor";

    /**Seconds between each sprite in this Conveyor's movement animation. */
    public virtual float MovementAnimationSpeed { get; } = .3f;

    /**Number of seconds it takes a GameObject to cross this Conveyor. */
    public virtual float ConveyorSpeed { get; } = .5f;

    /**An array of sprites, in order, that form this Conveyor's movement animation. */
    public Sprite[] MovementAnimationTrack { get; protected set; }

    /**The Coroutine constructed and used when animating this Conveyor. */
    public Coroutine MovementCoroutine { get; protected set; }

    /**This conveyor's Sprite Renderer component. */
    public SpriteRenderer ConveyorSpriteRenderer { get; protected set; }

    /**This conveyor's ConveyorScriptable component.*/
    public ConveyorScriptable Scriptable { get; protected set; }

    /**The number of times a GameObject on this conveyor moves closer to its target.*/
    public const int NUM_MOVEMENT_TICKS = 6;

    /**The direction of this Conveyor. */
    public Direction ConveyorDirection { get; set; }


    /** All items that can be dragged and dropped onto/into this Item.
 *  If any item can be dragged to this item, it contains one element "All"
 *  If no item can be dragged to this item, it contains no elements or is null. */
    public override string[] ItemsCanTakeByDragging { get; } = {};


    /** All items that can be moved onto/into this Item from an IMover.
     *  If any item can be moved to this item, it contains one element "All"
     *  If no item can be moved to this item, it contains no elements or is null. */
    public override string[] ItemsCanTakeByMovement { get; } = { "All" };

    /**Destroys an item on this conveyor.*/
    public override void DestroyMovedItem(GameObject item)
    {
        Destroy(item);
    }



    /**Moves item to its next tile's TargetPosition.*/
    public override void MoveItem(GameObject item)
    {
        if (!CanContinue(this)) DestroyMovedItem(item);
        else {
            FluidItem next = GameTileIn.NextGameTile.objectHolding.GetComponent<FluidItem>();
            StartCoroutine(MoveItemCoro(item, GameTileIn.NextGameTile.objectHolding.GetComponent<FluidItem>().GetTargetDestination()));
        }
    }

    /**Moves item to targetDestination.*/
    public override void MoveItem(GameObject item, Vector3 targetDestination)
    {
        if (!CanContinue(this)) DestroyMovedItem(item);
        else StartCoroutine(MoveItemCoro(item, targetDestination));
    }


    IEnumerator MoveItemCoro(GameObject item, Vector3 targetDestination)
    {
        if(GameTileIn.NextGameTile.objectHolding.GetComponent<Station>() != null)
        {
            Station next = GameTileIn.NextGameTile.objectHolding.GetComponent<Station>();
            if (!next.MoveToHolder(item))
            {
                DestroyMovedItem(item);
                yield break;
            }
        }

        Vector3 distanceToTravel = targetDestination - item.transform.position;
        Vector3 distancePerTick = distanceToTravel / NUM_MOVEMENT_TICKS;
        float secondsBetweenTick = ConveyorSpeed / NUM_MOVEMENT_TICKS;

        for (int i = 0; i < NUM_MOVEMENT_TICKS; i++)
        {
            item.transform.position += distancePerTick;
            yield return new WaitForSeconds(secondsBetweenTick);
        }

        GiveMovedItem(item, GameTileIn.NextGameTile.objectHolding.GetComponent<FluidItem>());
    }

    /**DEFINE ME*/
    public override void TakeMovedItem(GameObject item)
    {
        ProcessMovedItem(item);
    }

    public virtual void ProcessMovedItem(GameObject item)
    {
       MoveItem(item);
    }

    
    protected override void Start()
    {
        base.Start();
        ConveyorSpriteRenderer = GetComponent<SpriteRenderer>();
        Scriptable = (ConveyorScriptable)ItemGenerator.GetScriptableObject(NAME);
        SetUpAnimationTracks();
        MovementCoroutine = PlayAnimation(MovementAnimationTrack, MovementAnimationSpeed, ConveyorSpriteRenderer);
    }

    public override bool TakeDraggedItem(ItemScriptable item)
    {
        return false;
    }

    public override void ReleaseDraggedItem()
    {
        throw new NotImplementedException();
    }

    /**Gathers all animation sprites from Scriptable and stores them in this class.*/

    public virtual void SetUpAnimationTracks()
    {
        MovementAnimationTrack = Scriptable.conveyorMovementAnimationTrack;
    }


    /**Plays an animation and stores its Coroutine object.*/
    public Coroutine PlayAnimation(Sprite[] animationTrackToPlay, float secondsBetween, SpriteRenderer rendererToAnimate)
    {
        Coroutine coroToReturn = null;

        if (animationTrackToPlay == MovementAnimationTrack)
        {
            coroToReturn = StartCoroutine(PlayAnimationCoro(animationTrackToPlay, secondsBetween, rendererToAnimate));
        }
        return coroToReturn;
    }

    IEnumerator PlayAnimationCoro(Sprite[] animationTrackToPlay, float secondsBetween, SpriteRenderer rendererToAnimate)
    {
        while (true)
        {
            foreach(Sprite sprite in animationTrackToPlay)
            {
                ConveyorSpriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(secondsBetween);
            }
        }
    }

    /**Stops an existing Coroutine.*/
    public void StopAnimation(Coroutine animationToStop)
    {
        StopCoroutine(animationToStop);
    }

    /**Cashes an item in.*/
    public override void CashMovedItemIn(GameObject item)
    {

    }

}
