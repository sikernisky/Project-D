using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStation : Station
{
    public override string NAME { get; } = "PlateStation";

    public GameObject plateHolding;

    private GameObject emptyPlate;

    public override string[] ItemsCanTake { get; } = {
        "All" };

    public override float TimeToProcess { get; } = 1.0f;

    /**Set up ONE holder.*/
    public override void SetUpHolders()
    {


    }

    public void SpawnPlate()
    {
        GameObject spawnedPlate = Instantiate(Resources.Load("Prefabs/PlatePrefab") as GameObject, transform);
        spawnedPlate.transform.localPosition = new Vector2(0, .25f);
        plateHolding = spawnedPlate;
    }

    

  

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            MoveItem(plateHolding);
            SpawnPlate();
        }
    }


}