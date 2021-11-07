using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeStation : Station
{
    /**The name of this ServeStation. */
    public override string NAME { get; } = "ServeStation";

    /**The string list of items that this ServeStation can take by movement. */
    public override string[] ItemsCanTakeByMovement { get; } = {
        "All" };

    /**The string list of items that this ServeStation can take by dragging. */
    public override string[] ItemsCanTakeByDragging { get; } = {
        "Broccoli" };

    /**The time it takes this station to process an item by movement. */
    public override float TimeToHold { get; set; } = 3.5f;

    /**This ServeStation's left StationHolder. */
    public StationHolder leftHolder;

    /**The SpriteRenderer for the left holder's light. */
    public SpriteRenderer leftHolderLight;

    /**The SpriteRenderer for the right holder's light. */
    public SpriteRenderer rightHolderLight;

    /**This ServeStation's right StationHolder. */
    public StationHolder rightHolder;

    /**The closed holder Sprite for this ServeStation's holders. */
    public Sprite closedHolderSprite;

    /**The Sprite for this ServeStation's StationHolder's occupied state. */
    public Sprite holderLightOccupiedSprite;

    /**The Sprite for this ServeStation's StationHolder's vacant state. */
    public Sprite holderLightVacantSprite;

    /**The Coroutine for playing the steamer animation. */
    public Coroutine LeftHolderSteamAnimation { get; private set; }

    /**The Coroutine for playing the steamer animation. */
    public Coroutine RightHolderSteamAnimation { get; private set; }

    /**The animation track for the holder steam animation.  */
    public Sprite[] HolderSteamAnimationTrack;

    /**A FIFO list of this Station's StationHolders. */
    public List<StationHolder> ServeHolderQueue { get; protected set; }


    protected override void Start()
    {
        base.Start();
        ServeHolderQueue = new List<StationHolder>();
        Holders = new StationHolder[2];

        LeftHolderSteamAnimation = PlayAnimation(HolderSteamAnimationTrack, .2f, leftHolder.HolderSpriteRenderer);
        leftHolder.ItemsCanTakeByDragging = ItemsCanTakeByDragging;
        leftHolder.ItemsCanTakeByMovement = ItemsCanTakeByMovement;
        leftHolderLight.sprite = holderLightVacantSprite;
        Holders[0] = leftHolder;

        RightHolderSteamAnimation = PlayAnimation(HolderSteamAnimationTrack, .2f, rightHolder.HolderSpriteRenderer);
        rightHolder.ItemsCanTakeByDragging = ItemsCanTakeByDragging;
        rightHolder.ItemsCanTakeByMovement = ItemsCanTakeByMovement;
        rightHolderLight.sprite = holderLightVacantSprite;
        Holders[1] = rightHolder;

        RightHolderSteamAnimation = PlayAnimation(HolderSteamAnimationTrack, .2f, rightHolder.HolderSpriteRenderer);

    }

    public override bool CanDragTo(ItemScriptable itemBeingDragged)
    {
        if (leftHolder.Queued && rightHolder.Queued) return false;
        return base.CanDragTo(itemBeingDragged);
    }

    public override void HoldMovedItem(GameObject item)
    {
        StartCoroutine(HoldItemCoro(item));
    }

    
    public override bool MoveToHolder(GameObject item)
    {
        if (leftHolder.transform.position == GetTargetDestination())
        {
            leftHolder.HoldItem(item);
            ServeHolderQueue.Add(leftHolder);
            return true;
        }
        else if (rightHolder.transform.position == GetTargetDestination())
        {
            rightHolder.HoldItem(item);
            ServeHolderQueue.Add(rightHolder);
            return true;
        }
        else return false;
    }

    

    public override bool TakeDraggedItem(ItemScriptable item)
    {
        base.TakeDraggedItem(item);
        if (!leftHolder.Queued)
        {
            leftHolder.QueueItem(item);
            leftHolderLight.sprite = holderLightOccupiedSprite;
            return true;
        }
        else if (!rightHolder.Queued)
        {
            rightHolder.QueueItem(item);
            rightHolderLight.sprite = holderLightOccupiedSprite;
            return true;
        }
        return false;
    }

    IEnumerator HoldItemCoro(GameObject item)
    {
        yield return new WaitForSeconds(TimeToHold);
        ServeHolderQueue[0].ReleaseHeldItem();
        ServeHolderQueue.RemoveAt(0);
        ProcessMovedItem(item);
    }

    public override Vector3 GetTargetDestination()
    {
        Vector3 correctPosition = new Vector3(-1,-1,-1);
        if (!leftHolder.Occupied && !rightHolder.Occupied) correctPosition = leftHolder.transform.position;
        else if (leftHolder.Occupied && !rightHolder.Occupied) correctPosition = rightHolder.transform.position;
        else if (!leftHolder.Occupied && rightHolder.Occupied) correctPosition = leftHolder.transform.position;
        return correctPosition;
    }




}
