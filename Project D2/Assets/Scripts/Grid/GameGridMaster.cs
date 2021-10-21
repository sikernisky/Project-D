using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridMaster : MonoBehaviour
{
    /**An array of sprites for ground/base tiles. Assigned in the inspector. */
    public Sprite[] baseTileArray;

    /**The main Game Grid that we operate on.*/
    public GameGrid GameGrid { get; private set; }

    void Start()
    {
        GameGrid grid = new GameGrid(30, 30, 2, baseTileArray, transform);
        GameGrid = grid;
    }



}
