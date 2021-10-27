using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Item, IMover, IAnimator
{
    /**Name of this conveyor. Corresponds to its Class name. */
    public override string NAME { get; } = "Conveyor";

    /**Seconds between each sprite in this Conveyor's movement animation. */
    public virtual float MovementAnimationSpeed { get; } = .3f;

    /**An array of sprites, in order, that form this Conveyor's movement animation. */
    public Sprite[] MovementAnimationSpriteTrack { get; protected set; }

    public Coroutine MovementCoroutine { get; protected set; }

    /**This conveyor's Sprite Renderer component. */
    public SpriteRenderer ConveyorSpriteRenderer { get; protected set; }

    /**This conveyor's ConveyorScriptable counterpart.*/
    public ConveyorScriptable Scriptable { get; protected set; }

    /**Destroys an item on this conveyor.*/
    public virtual void DestroyItem(GameObject item)
    {
       
    }

    /**Gives an item it holds to another IMover.*/
    public virtual void GiveItem(GameObject item)
    {

    }

    /**Moves this item across the conveyor.*/
    public virtual void MoveItem(GameObject item)
    {

    }

    /**DEFINE ME*/
    public virtual void TakeItem(GameObject item)
    {

    }

    /**Gathers all animation sprites from Scriptable and stores them in this class.*/

    public virtual void SetUpAnimationTracks() {
        MovementAnimationSpriteTrack = Scriptable.conveyorMovementAnimationTrack;
    }
    
    public void Start()
    {
        ConveyorSpriteRenderer = GetComponent<SpriteRenderer>();
        Scriptable = (ConveyorScriptable)ItemGenerator.GetScriptableObject(NAME);
        SetUpAnimationTracks();
        PlayAnimation(MovementAnimationSpriteTrack, MovementAnimationSpeed);
    }

    /**Plays an animation and stores its Coroutine object.*/
    public void PlayAnimation(Sprite[] animationTrackToPlay, float secondsBetween)
    {
        if(animationTrackToPlay == MovementAnimationSpriteTrack)
        {
            MovementCoroutine = StartCoroutine(PlayAnimationCoro(animationTrackToPlay, secondsBetween));
        }
    }

    IEnumerator PlayAnimationCoro(Sprite[] animationTrackToPlay, float secondsBetween)
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
