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

    public override float TimeToProcess { get; } = 1.0f;

    public StationHolder leftHolder;

    public StationHolder rightHolder;

    public Sprite closedHolderSprite;

    protected override void Start()
    {
        base.Start();
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


    public override void HoldMovedItem(GameObject item)
    {
        StartCoroutine(HoldItemCoro(item));
    }

    public override void ProcessMovedItem(GameObject item)
    {
        base.ProcessMovedItem(item);
    }

    IEnumerator HoldItemCoro(GameObject item)
    {
        StationHolder holder = null;

        if (!leftHolder.Occupied) holder = leftHolder.HoldItem(item);
        else if (!rightHolder.Occupied) holder = rightHolder.HoldItem(item);
        else { DestroyMovedItem(item); yield break; }

        yield return new WaitForSeconds(TimeToProcess);

        if(holder.Occupied) holder.ReleaseHeldItem();
        ProcessMovedItem(item);
    }

    public override void TakeDraggedItem(ItemScriptable item)
    {
        if (!leftHolder.Queued)
        {
            leftHolder.QueueItem(item, closedHolderSprite);
        }
    }




}
