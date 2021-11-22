using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Customer> customerQueue;

    public static string customerPrefabSource = "Prefabs/CharacterPrefabs/BubbyPrefab";

    // Start is called before the first frame update
    void Start()
    {
        SetUpCustomerQueue(SaveMaster.saveData.levelToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnCustomerAtRandomLocation();
        }
    }

    private void SpawnCustomerAtRandomLocation()
    {
        GameObject spawnedCustomer = Instantiate(Resources.Load(customerPrefabSource) as GameObject);
        spawnedCustomer.transform.SetParent(transform);
        List<GameTile> spawnableTiles = new List<GameTile>();
        foreach(GameTile tile in GameGridMaster.BaseGameGrid.GridArray)
        {
            if (tile.Walkable) spawnableTiles.Add(tile);
        }
        GameTile randomTile = spawnableTiles[Random.Range(0, spawnableTiles.Count - 1)];
        spawnedCustomer.transform.position = new Vector3(randomTile.WorldX, randomTile.WorldY, 5);
    }

    private void SetUpCustomerQueue(int levelNumber)
    {
        customerQueue = new List<Customer>();
        switch (levelNumber)
        {
            case 1:


                break;
            default:
                break;

        }
    }



}
