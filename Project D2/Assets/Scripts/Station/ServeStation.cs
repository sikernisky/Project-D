using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeStation : Station
{
    public override string NAME { get; } = "ServeStation";

    public override string[] ItemsCanTakeByMovement { get; } = {
        "All" };

    public override string[] ItemsCanTakeByDragging { get; } = {
        "Broccoli" };

    public override float TimeToProcess { get; } = 3.5f;

    public StationHolder leftHolder;

    public StationHolder rightHolder;

    public Sprite closedHolderSprite;

    public List<StationHolder> HolderQueue { get; private set; }

    protected override void Start()
    {
        base.Start();
        HolderQueue = new List<StationHolder>();
        Holders = new StationHolder[2];
        Holders[0] = leftHolder;
        Holders[1] = rightHolder;
        foreach (StationHolder holder in Holders)
        {
            holder.HolderAnimation = PlayAnimation(HolderAnimationTrack, .2f, holder.HolderSpriteRenderer);
            holder.ItemsCanTakeByDragging = ItemsCanTakeByDragging;
            holder.ItemsCanTakeByMovement = ItemsCanTakeByMovement;
        }
    }

    void Update()
    {

    }

    public override void HoldMovedItem(GameObject item)
    {
        StartCoroutine(HoldItemCoro(item));
    }

    public override void MoveToHolder(GameObject item)
    {
        if (leftHolder.transform.position == GetTargetDestination())
        {
            leftHolder.HoldItem(item);
            HolderQueue.Add(leftHolder);
        }
        else if (rightHolder.transform.position == GetTargetDestination())
        {
            rightHolder.HoldItem(item);
            HolderQueue.Add(rightHolder);
        }
        else DestroyMovedItem(item);
    }

    public override void ProcessMovedItem(GameObject item)
    {
        base.ProcessMovedItem(item);
    }

    IEnumerator HoldItemCoro(GameObject item)
    {
        yield return new WaitForSeconds(TimeToProcess);
        HolderQueue[0].ReleaseHeldItem();
        HolderQueue.RemoveAt(0);
        ProcessMovedItem(item);
    }

    public override void TakeDraggedItem(ItemScriptable item)
    {
        if (!leftHolder.Queued)
        {
            leftHolder.QueueItem(item, closedHolderSprite);
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
