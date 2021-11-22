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
         };

    /**This ServeStation's left StationHolder. */
    public StationHolder leftHolder;

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
        Holders[0] = leftHolder;

        RightHolderSteamAnimation = PlayAnimation(HolderSteamAnimationTrack, .2f, rightHolder.HolderSpriteRenderer);
        rightHolder.ItemsCanTakeByDragging = ItemsCanTakeByDragging;
        rightHolder.ItemsCanTakeByMovement = ItemsCanTakeByMovement;
        Holders[1] = rightHolder;


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GivePlateToCustomer(leftHolder);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GivePlateToCustomer(rightHolder);
        }
    }


    public override void MoveItem(GameObject item)
    {
        return;
    }

    public override void MoveItem(GameObject item, Vector3 targetDestination)
    {
        return;
    }

    public override bool CanDragTo(ItemScriptable itemBeingDragged)
    {
        if (leftHolder.Queued && rightHolder.Queued) return false;
        return base.CanDragTo(itemBeingDragged);
    }


    public override void TakeMovedItem(GameObject item)
    {
    }

    public override bool MoveToHolder(GameObject item)
    {
        Debug.Log("Called");
        if (leftHolder.transform.position == GetTargetDestination())
        {
            StopCoroutine(LeftHolderSteamAnimation);
            leftHolder.HolderSpriteRenderer.sprite = closedHolderSprite;
            HideHeldItem(item);
            leftHolder.HoldItem(item);
            ServeHolderQueue.Add(leftHolder);
            return true;
        }
        else if (rightHolder.transform.position == GetTargetDestination())
        {
            StopCoroutine(RightHolderSteamAnimation);
            rightHolder.HolderSpriteRenderer.sprite = closedHolderSprite;
            HideHeldItem(item);
            rightHolder.HoldItem(item);
            ServeHolderQueue.Add(rightHolder);
            return true;
        }
        else return false;
    }

    

    public override bool TakeDraggedItem(ItemScriptable item)
    {
        /*base.TakeDraggedItem(item);
        if (!leftHolder.Queued)
        {
            StopCoroutine(LeftHolderSteamAnimation);
            leftHolder.HolderSpriteRenderer.sprite = closedHolderSprite;
            leftHolder.QueueItem(item);
            leftHolder.Occupied = true;
            return true;
        }
        else if (!rightHolder.Queued)
        {
            StopCoroutine(RightHolderSteamAnimation);
            rightHolder.HolderSpriteRenderer.sprite = closedHolderSprite;
            rightHolder.QueueItem(item);
            rightHolder.Occupied = true;
            return true;
        } */

        return false;
    }

    public void GivePlateToCustomer(StationHolder stationHolder)
    {
        if (stationHolder == leftHolder) {
            DestroyMovedItem(leftHolder.ItemHolding);
            leftHolder.ReleaseHeldItem();
            LeftHolderSteamAnimation = PlayAnimation(HolderSteamAnimationTrack, .2f, leftHolder.HolderSpriteRenderer);
        }
        else if (stationHolder == rightHolder) {
            DestroyMovedItem(rightHolder.ItemHolding);
            rightHolder.ReleaseHeldItem();
            RightHolderSteamAnimation = PlayAnimation(HolderSteamAnimationTrack, .2f, rightHolder.HolderSpriteRenderer);
        }
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
