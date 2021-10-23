using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridMaster : MonoBehaviour
{
    private const float cellSize = 2f;

    /**An array of sprites for ground/base tiles. Assigned in the inspector. */
    public Sprite[] baseTileArray;

    /**The Base Game Grid that we operate on.*/
    public GameGrid BaseGameGrid { get; private set; }

    /**The Moveables Game Grid that we operate on.*/
    public GameGrid MoveablesGameGrid { get; private set; }

    /**The Placeables Game Grid that we operate on.*/
    public GameGrid PlaceablesGameGrid { get; private set; }

    /**The Game Grid that detects clicks and performs actions with other Grids.*/
    public GameGrid ListenerGameGrid { get; private set; }


    void Start()
    {
        SaveMaster.saveData.levelToLoad = 1;
        GenerateLevel(SaveMaster.saveData.levelToLoad);
    }

    private GameGrid CreateGroundTilemap(int width, int height)
    {
        GameObject GroundTilemapParent = new GameObject("GroundTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, GroundTilemapParent.transform);
        return grid;
    }

    private GameGrid CreateMoveablesTilemap(int width, int height)
    {
        GameObject MoveablesTilemapParent = new GameObject("MoveablesTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, MoveablesTilemapParent.transform, 1);
        return grid;
    }

    private GameGrid CreatePlaceablesTilemap(int width, int height)
    {
        GameObject PlaceablesTilemap = new GameObject("PlaceablesTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, transform, 2);
        return grid;
    }

    private void GenerateLevel(int levelNumber)
    {
        int width;
        int height;

        switch (levelNumber)
        {
            case 1:
                width = 25;
                height = 25;
                break;
            default:
                width = 20;
                height = 20;
                return;
        }

        BaseGameGrid = CreateGroundTilemap(width, height);
        BaseGameGrid.FillAllTiles(baseTileArray);

        MoveablesGameGrid = CreateMoveablesTilemap(width, height);

        PlaceablesGameGrid = CreatePlaceablesTilemap(width, height);

        ListenerGameGrid = CreatePlaceablesTilemap(width, height);
        BaseGameGrid.FillAllTiles(null);


        switch (levelNumber)
        {
            case 1:
                MoveablesGameGrid.FillTileInGrid(new Vector2(0, 0), "DefaultConveyor");
                MoveablesGameGrid.FillTileInGrid(new Vector2(10, 20), "DefaultConveyor");
                MoveablesGameGrid.FillTileInGrid(new Vector2(13, 43), "DefaultConveyor");
                break;
            default:
                return;
        }
    }



}
