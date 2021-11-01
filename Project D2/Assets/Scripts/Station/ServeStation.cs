using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeStation : Station
{
    public override string NAME { get; } = "ServeStation";

    public override string[] ItemsCanTake { get; } = {
        "All" };

    public override float TimeToProcess { get; } = 1.0f;


    protected override void Start()
    {
        base.Start();
        foreach (StationHolder holder in Holders)
        {
            holder.HolderAnimation = PlayAnimation(HolderAnimationTrack, .2f, holder.HolderSpriteRenderer);
        }
    }

    /**Set up TWO holders: a left holder and a right holder. */
    public override void SetUpHolders()
    {
        Holders = new StationHolder[2];
        Holders[0] = transform.GetChild(0).GetComponent<StationHolder>();
        Holders[1] = transform.GetChild(1).GetComponent<StationHolder>();
    }

    public override void HoldItem(GameObject item)
    {
        StartCoroutine(HoldItemCoro(item));
    }

    IEnumerator HoldItemCoro(GameObject item)
    {
        StationHolder holder = null;
        if (!Holders[0].HoldingObject) holder = Holders[0].HoldItem(item);
        else if (!Holders[1].HoldingObject) holder = Holders[1].HoldItem(item);
        if (item.GetComponent<SpriteRenderer>() != null) item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);

        yield return new WaitForSeconds(TimeToProcess);

        if (item.GetComponent<SpriteRenderer>() != null) item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);


        if (holder != null) holder.ReleaseItem();
        ProcessItem(item);
    }



}
