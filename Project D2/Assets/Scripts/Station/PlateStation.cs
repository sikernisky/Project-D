using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStation : Station
{
    /**The name of this Station. */
    public override string NAME { get; } = "PlateStation";

    /**The Plate GameObject this PlateStation is holding.*/
    public GameObject plateHolding;

    /**An empty Plate GameObject.*/
    private GameObject emptyPlate;

    /** All items that can be moved onto/into this Item from an IMover.
     *  If any item can be moved to this item, it contains one element "All"
     *  If no item can be moved to this item, it contains no elements or is null. */
    public override string[] ItemsCanTakeByMovement { get; } = {
        "All" };

    /**The time it takes to process an Item. */
    public override float TimeToProcess { get; } = 1.0f;

    /**Set up ONE holder.*/
    public override void SetUpHolders()
    {


    }

    public void SpawnPlate()
    {
        GameObject spawnedPlate = Instantiate(Resources.Load("Prefabs/PlatePrefab") as GameObject, transform);
        spawnedPlate.transform.localPosition = new Vector2(0, .25f);
        spawnedPlate.transform.SetParent(transform);
        plateHolding = spawnedPlate;
    }

    public override void ProcessMovedItem(GameObject item)
    {
        base.ProcessMovedItem(item);
    }

    public override bool TakeDraggedItem(ItemScriptable item)
    {
        FoodScriptable itemFood = item as FoodScriptable;
        if(itemFood != null)
        {
            plateHolding.GetComponent<Plate>().AddFoodToPlate(itemFood);
            HoldMovedItem(plateHolding);
            SpawnPlate();
            return true;
        }
        return false;
    }

}