using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridMaster : MonoBehaviour
{
    public const float cellSize = 2f;

    /**An array of sprites for ground/base tiles. Assigned in the inspector. */
    public Sprite[] baseTileArray;

    /**The Base Game Grid that we operate on.*/
    public static GameGrid BaseGameGrid { get; private set; }

    /**The Moveables Game Grid that we operate on.*/
    public static GameGrid MoveablesGameGrid { get; private set; }

    /**The Placeables Game Grid that we operate on.*/
    public static GameGrid PlaceablesGameGrid { get; private set; }

    /**The Game Grid that detects clicks and performs actions with other Grids.*/
    public static GameGrid ListenerGameGrid { get; private set; }

    /**The width of all grids in this scene. */
    public static int GridWidth;

    /**The height of all grids in this scene. */
    public static int GridHeight;


    void Start()
    {
        SaveMaster.saveData.levelToLoad = 1;
        GenerateLevel(SaveMaster.saveData.levelToLoad);
    }

    private GameGrid CreateGroundTilemap(int width, int height)
    {
        GameObject GroundTilemapParent = new GameObject("GroundTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, GroundTilemapParent.transform);
        grid.FillAllTiles(baseTileArray);
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
        GameGrid grid = new GameGrid(width, height, cellSize, PlaceablesTilemap.transform, 2);
        return grid;
    }

    private void GenerateLevel(int levelNumber)
    {

        switch (levelNumber)
        {
            case 1:
                GridWidth = 40;
                GridHeight = 40;
                break;
            default:
                GridWidth = 20;
                GridHeight = 20;
                return;
        }

        SpawnAllGrids(GridWidth, GridHeight);

        switch (levelNumber)
        {
            case 1:
                MoveablesGameGrid.FillTileInGrid(new Vector2(25, 21), new Vector2(1, 1), "Prefabs/StationPrefabs/PlateStationPrefab");
                MoveablesGameGrid.FillTileInGrid(new Vector2(25, 22), new Vector2(1, 1), "Prefabs/ConveyorPrefabs/DefaultConveyorPrefab");
                MoveablesGameGrid.FillTileInGrid(new Vector2(25, 23), new Vector2(1, 1), "Prefabs/ConveyorPrefabs/DeluxeConveyorPrefab");
                MoveablesGameGrid.FillTileInGrid(new Vector2(25, 24), new Vector2(1, 1), "Prefabs/ConveyorPrefabs/DefaultConveyorPrefab");
                MoveablesGameGrid.FillTileInGrid(new Vector2(25, 25), new Vector2(3, 3), "Prefabs/StationPrefabs/ServeStationPrefab");

                break;
            default:
                return;
        }
    }

    private void SpawnAllGrids(int width, int height)
    {
        BaseGameGrid = CreateGroundTilemap(width, height);
        MoveablesGameGrid = CreateMoveablesTilemap(width, height);
        PlaceablesGameGrid = CreatePlaceablesTilemap(width, height);
    }
}
