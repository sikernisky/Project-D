using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoastStation : Station
{
    /**
     * The player drags to burners and plates move to ovens. 
     * */


    /**The name of this ServeStation. */
    public override string NAME { get; } = "RoastStation";

    /**The string list of items that this ServeStation can take by movement. */
    public override string[] ItemsCanTakeByMovement { get; } = {
        "All" };

    /**The string list of items that this ServeStation can take by dragging. */
    public override string[] ItemsCanTakeByDragging { get; } = {
        "All" };

    /**The time it takes this RoastStation to cook an item in its oven. */
    public override float TimeToHold { get; set; } = 5f;

    /**The SpriteRenderer for the left oven's timer. */
    public SpriteRenderer leftOvenTimer;

    /**The SpriteRenderer for the right oven's timer. */
    public SpriteRenderer rightOvenTimer;

    /**The animation track for the oven timer fill up animation.*/
    public Sprite[] timerFillUpAnimationSpriteTrack;

    /**This RoastStation's left oven StationHolder. */
    public StationHolder leftOvenHolder;

    /**This RoastStation's right oven StationHolder. */
    public StationHolder rightOvenHolder;

    /**The sprite used when an oven is NOT occupied. */
    public Sprite openOvenSprite;

    /**The sprite used when an oven is occupied. */
    public Sprite closedOvenSprite;

    /**This RoastStation's left burner StationHolder. */
    public StationHolder leftBurnerHolder;

    /**This RoastStation's left burner pan SpriteRenderer. */
    public SpriteRenderer leftBurnerPan;

    /**This RoastStation's right burner StationHolder. */
    public StationHolder rightBurnerHolder;

    /**This RoastStation's right burner pan SpriteRenderer. */
    public SpriteRenderer rightBurnerPan;

    /**The Coroutine for playing the passive left burner animation. */
    public Coroutine LeftBurnerPassiveAnimation { get; private set; }

    /**The Coroutine for playing the passive right burner animation. */
    public Coroutine RightBurnerPassiveAnimation { get; private set; }

    /**The animation track for the burner passive animation.  */
    public Sprite[] burnerPassiveAnimationSpriteTrack;

    /**A FIFO list of this Station's StationHolders. */
    public List<StationHolder> OvenHolderQueue { get; protected set; }

    /**A FIFO list of this Station's StationHolders. */
    public List<StationHolder> BurnerHolderQueue { get; protected set; }

    /**The last StationHolder (oven) an item was moved to. */
    public StationHolder MostRecentMovedOven { get; private set; }



    protected override void Start()
    {
        base.Start();
        OvenHolderQueue = new List<StationHolder>();
        BurnerHolderQueue = new List<StationHolder>();
        Holders = new StationHolder[4];

        LeftBurnerPassiveAnimation = PlayAnimation(burnerPassiveAnimationSpriteTrack, .2f, leftBurnerHolder.HolderSpriteRenderer);
        leftBurnerHolder.ItemsCanTakeByDragging = ItemsCanTakeByDragging;
        leftOvenHolder.ItemsCanTakeByMovement = ItemsCanTakeByMovement;
        Holders[0] = leftBurnerHolder;
        Holders[1] = leftOvenHolder;


        RightBurnerPassiveAnimation = PlayAnimation(burnerPassiveAnimationSpriteTrack, .2f, rightBurnerHolder.HolderSpriteRenderer);
        rightBurnerHolder.ItemsCanTakeByDragging = ItemsCanTakeByDragging;
        rightOvenHolder.ItemsCanTakeByMovement = ItemsCanTakeByMovement;
        Holders[2] = rightBurnerHolder;
        Holders[3] = rightOvenHolder;
    }

    public override bool CanDragTo(ItemScriptable itemBeingDragged)
    {
        if (leftBurnerHolder.Queued && rightBurnerHolder.Queued) return false;
        return base.CanDragTo(itemBeingDragged);
    }

    public override void HoldMovedItem(GameObject item)
    {
        StartCoroutine(HoldItemCoro(item));
    }

    IEnumerator HoldItemCoro(GameObject item)
    {
        SpriteRenderer timerToFill = null;
        StationHolder ovenToModify = null;
        if (MostRecentMovedOven == leftOvenHolder)
        {
            timerToFill = leftOvenTimer;
            ovenToModify = leftOvenHolder;
        }
        else
        {
            timerToFill = rightOvenTimer;
            ovenToModify = rightOvenHolder;
        }
        ovenToModify.HolderSpriteRenderer.sprite = closedOvenSprite;
        HideHeldItem(item);

        for(int i = 0; i < timerFillUpAnimationSpriteTrack.Length; i++)
        {
            timerToFill.sprite = timerFillUpAnimationSpriteTrack[i];
            yield return new WaitForSeconds(TimeToHold/(timerFillUpAnimationSpriteTrack.Length - 1));
        }
        ovenToModify.HolderSpriteRenderer.sprite = openOvenSprite;
        ShowHeldItem(item);
        timerToFill.sprite = timerFillUpAnimationSpriteTrack[0];
        OvenHolderQueue[0].ReleaseHeldItem();
        OvenHolderQueue.RemoveAt(0);
        ProcessMovedItem(item);

    }

    public override Vector3 GetTargetDestination()
    {
        Vector3 correctPosition = new Vector3(-1, -1, -1);
        if (!leftOvenHolder.Occupied && !rightOvenHolder.Occupied) correctPosition = leftOvenHolder.transform.position;
        else if (leftOvenHolder.Occupied && !rightOvenHolder.Occupied) correctPosition = rightOvenHolder.transform.position;
        else if (!leftOvenHolder.Occupied && rightOvenHolder.Occupied) correctPosition = leftOvenHolder.transform.position;
        return correctPosition;
    }

    public override bool TakeDraggedItem(ItemScriptable item)
    {
        if (item as FoodScriptable == null) return false; // Roast Stations can only take FOODs.
        FoodScriptable foodItem = (FoodScriptable)item;

        base.TakeDraggedItem(item);
        if (!leftBurnerHolder.Queued)
        {
            leftBurnerHolder.QueueItem(item);
            leftBurnerPan.sprite = foodItem.roastStationSprite;
            return true;
        }
        else if (!rightBurnerHolder.Queued)
        {
            rightBurnerHolder.QueueItem(item);
            rightBurnerPan.sprite = foodItem.roastStationSprite;
            return true;
        }
        return false;
    }


    public override bool MoveToHolder(GameObject item)
    {
        if (leftOvenHolder.transform.position == GetTargetDestination())
        {
            leftOvenHolder.HoldItem(item);
            OvenHolderQueue.Add(leftOvenHolder);
            MostRecentMovedOven = leftOvenHolder;
            return true;
        }
        else if (rightOvenHolder.transform.position == GetTargetDestination())
        {
            rightOvenHolder.HoldItem(item);
            OvenHolderQueue.Add(rightOvenHolder);
            MostRecentMovedOven = rightOvenHolder;
            return true;
        }
        else return false;
    }

    public override void PutHeldItemOnPlate(Plate plateToPutOn)
    {

        if (leftBurnerHolder.Queued)
        {
            Food foodToAdd = (Food)ItemGenerator.GetItemFromString(leftBurnerHolder.ItemQueuedScriptable.itemClassName);
            foodToAdd.Roasted = true;

            plateToPutOn.AddFoodToPlate(foodToAdd, ((FoodScriptable)leftBurnerHolder.ItemQueuedScriptable).roastPlateSprite);
            if(plateToPutOn.gameObject.GetComponent(ItemGenerator.GetClassFromString(leftBurnerHolder.ItemQueuedScriptable.itemClassName)) != null)
            {
                plateToPutOn.gameObject.GetComponent<Food>().Roasted = true;
            }
            else
            {
                plateToPutOn.gameObject.AddComponent(ItemGenerator.GetClassFromString(leftBurnerHolder.ItemQueuedScriptable.itemClassName));
                try
                {
                    plateToPutOn.gameObject.GetComponent<Food>().Roasted = true;
                }
                catch
                {
                    Debug.Log("Component added to plate does not have field Roasted.");
                }
            }

            leftBurnerPan.sprite = null;
            leftBurnerHolder.ReleaseQueuedItem();
        }
        else if (rightBurnerHolder.Queued)
        {
            Food foodToAdd = (Food)ItemGenerator.GetItemFromString(rightBurnerHolder.ItemQueuedScriptable.itemClassName);
            foodToAdd.Roasted = true;

            plateToPutOn.AddFoodToPlate(foodToAdd, ((FoodScriptable)rightBurnerHolder.ItemQueuedScriptable).roastPlateSprite);
            if (plateToPutOn.gameObject.GetComponent(ItemGenerator.GetClassFromString(rightBurnerHolder.ItemQueuedScriptable.itemClassName)) != null)
            {
                plateToPutOn.gameObject.GetComponent<Food>().Roasted = true;
            }
            else
            {
                plateToPutOn.gameObject.AddComponent(ItemGenerator.GetClassFromString(rightBurnerHolder.ItemQueuedScriptable.itemClassName));
                try
                {
                    plateToPutOn.gameObject.GetComponent<Food>().Roasted = true;
                }
                catch
                {
                    Debug.Log("Component added to plate does not have field Roasted.");
                }
            }
            rightBurnerPan.sprite = null;
            rightBurnerHolder.ReleaseQueuedItem();
        }
    }

    public override void ProcessMovedItem(GameObject item)
    {
        if (item.GetComponent<Plate>() != null) PutHeldItemOnPlate(item.GetComponent<Plate>());
        base.ProcessMovedItem(item);

    }



}
