using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Item, IAnimator
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

    /**Destroys an item on this conveyor.*/
    public override void DestroyItem(GameObject item)
    {
        Destroy(item);
    }

    /**Moves this item across the conveyor.*/
    public override void MoveItem(GameObject item)
    {
        StartCoroutine(MoveItemCoro(item));
    }

    IEnumerator MoveItemCoro(GameObject item)
    {
        Vector3 targetDestination = GameTileIn.NextGameTile.objectHolding.transform.position;
        Vector3 distanceToTravel = targetDestination - item.transform.position;
        Vector3 distancePerTick = distanceToTravel / NUM_MOVEMENT_TICKS;
        float secondsBetweenTick = ConveyorSpeed / NUM_MOVEMENT_TICKS;

        for (int i = 0; i < NUM_MOVEMENT_TICKS; i++)
        {
            item.transform.position += distancePerTick;
            yield return new WaitForSeconds(secondsBetweenTick);
        }

        GiveItem(item, GameTileIn.NextGameTile.objectHolding.GetComponent<Item>());
    }

    /**DEFINE ME*/
    public override void TakeItem(GameObject item)
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector2(0, 0);

        ProcessItem(item);
    }

    public virtual void ProcessItem(GameObject item)
    {
        MoveItem(item);
    }

    /**Cashes an item in.*/
    public override void CashItemIn(GameObject item)
    {

    }

    /**Gathers all animation sprites from Scriptable and stores them in this class.*/

    public virtual void SetUpAnimationTracks() {
        MovementAnimationTrack = Scriptable.conveyorMovementAnimationTrack;
    }
    
    public void Start()
    {
        ConveyorSpriteRenderer = GetComponent<SpriteRenderer>();
        Scriptable = (ConveyorScriptable)ItemGenerator.GetScriptableObject(NAME);
        SetUpAnimationTracks();
        MovementCoroutine = PlayAnimation(MovementAnimationTrack, MovementAnimationSpeed, ConveyorSpriteRenderer);
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
}
