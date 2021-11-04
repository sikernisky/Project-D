using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridMaster : MonoBehaviour
{
    /**The cell size of created GameGrids. */
    public const float cellSize = 2f;

    /**An array of sprites for ground/base tiles. Assigned in the inspector. */
    public Sprite[] baseTileArray;

    /** The wooden floor Sprite of the customer area. Assigned in the inspector. */
    public Sprite woodFloor;

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


    /**Creates a GameGrid with width width and height height.
     (1) width: the width of this grid.
     (2) height: the height of this grid. */
    private GameGrid CreateGroundTilemap(int width, int height)
    {
        GameObject GroundTilemapParent = new GameObject("GroundTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, GroundTilemapParent.transform);
        grid.CreateAllTiles(baseTileArray);
        return grid;
    }

    /**Creates a GameGrid with width width and height height.
     (1) width: the width of this grid.
     (2) height: the height of this grid. */
    private GameGrid CreateMoveablesTilemap(int width, int height)
    {
        GameObject MoveablesTilemapParent = new GameObject("MoveablesTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, MoveablesTilemapParent.transform, 1);
        return grid;
    }

    /**Creates a GameGrid with width width and height height.
     (1) width: the width of this grid.
     (2) height: the height of this grid. */
    private GameGrid CreatePlaceablesTilemap(int width, int height)
    {
        GameObject PlaceablesTilemap = new GameObject("PlaceablesTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, PlaceablesTilemap.transform, 2);
        return grid;
    }

    /**Creates a GameGrid with width width and height height.
     - Calls CreateRealTile() on each GameTile in this grid.
     - Adds a ListenerTile component to each tile.
     - Calls SetCoordinates() on each ListenerTile. 
     (1) width: the width of this grid.
     (2) height: the height of this grid. */
    private GameGrid CreateListenerTilemap(int width, int height)
    {
        GameObject ListenerTilemap = new GameObject("ListenerTilemap");
        GameGrid grid = new GameGrid(width, height, cellSize, ListenerTilemap.transform, 3);
        foreach(GameTile tile in grid.GridArray)
        {
            tile.CreateRealTile(null);
            tile.objectHolding.AddComponent<ListenerTile>().tileGameTile = tile;
            tile.objectHolding.GetComponent<ListenerTile>().SetCoordinates();
        }
        return grid;
    }

    /**A massive function used to build levels. Results vary based on levelNumber.
     (1) levelNumber: the level to load. */
    private void GenerateLevel(int levelNumber)
    {
        Conveyor.Direction north = Conveyor.Direction.North;
        Conveyor.Direction east = Conveyor.Direction.East;
        //Conveyor.Direction south = Conveyor.Direction.South;
        Conveyor.Direction west = Conveyor.Direction.West;


        switch (levelNumber)
        {
            case 1:
                GridWidth = 25;
                GridHeight = 25;
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
                //Fill a tile at (13,19) with size (3,2) and set its next tile to (14,20)
                MoveablesGameGrid.FillTilePrefab(3,2,3,2,ItemGenerator.PlateStationPath, "PlateStation", north, 4, 3);
                MoveablesGameGrid.FillTilePrefab(4,3, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 4, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 5, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 6, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(3, 7, 3, 2, ItemGenerator.ServeStationPath, "ServeStation", north, 4, 8);
                MoveablesGameGrid.FillTilePrefab(4, 8, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 9, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 10, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 11, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 12, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 13, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 14, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 15, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTilePrefab(4, 16, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);



                SpawnFenceBoundaries(0, true, 6);



                break;
            default:
                return;
        }

    }

    /**Generates the GameGrids for static fields BaseGameGrid, MoveablesGameGrid, PlaceablesGameGrid, and ListenerGameGrid.
     (1) width: the width of each GameGrid.
     (2) height: the height of each GameGrid. */
    private void SpawnAllGrids(int width, int height)
    {
        BaseGameGrid = CreateGroundTilemap(width, height);
        MoveablesGameGrid = CreateMoveablesTilemap(width, height);
        PlaceablesGameGrid = CreatePlaceablesTilemap(width, height);
        ListenerGameGrid = CreateListenerTilemap(width, height);
    }


    /**Spawns a fence boundary numTilesOutsideOfBoundary tiles from the edge of the map.
     * (1) numTilesOutsideOfBoundary: the number of tiles away from the edge of all sides the boundary is spawned, >= 0*/
    private void SpawnFenceBoundaries(int numTilesOutsideOfBoundary, bool spawnCustomerArea=true, int customerAreaSize = 0)
    {
        if (numTilesOutsideOfBoundary < 0) return;

        int yTopBoundary = GridHeight - numTilesOutsideOfBoundary -1;
        int yBotBoundary = numTilesOutsideOfBoundary;

        int xLeftBoundary = numTilesOutsideOfBoundary;
        int xRightBoundary = GridWidth - numTilesOutsideOfBoundary -1;

        // Spawn horz and verts

        for(int i = xLeftBoundary; i < xRightBoundary; i++)
        {
            BaseGameGrid.FillTilePrefab(i, yTopBoundary, 1, 1, ItemGenerator.SingleHorizontalFencePath, "SingleHorizontalFence", Conveyor.Direction.North);
            BaseGameGrid.FillTilePrefab(i, yBotBoundary, 1, 1, ItemGenerator.SingleHorizontalFencePath, "SingleHorizontalFence", Conveyor.Direction.North);
        }

        for(int i = yBotBoundary; i < yTopBoundary; i++)
        {

            BaseGameGrid.FillTilePrefab(xLeftBoundary,i, 1, 1, ItemGenerator.SingleVerticalFencePath, "SingleVerticalFence", Conveyor.Direction.North);
            BaseGameGrid.FillTilePrefab(xRightBoundary, i, 1, 1, ItemGenerator.SingleVerticalFencePath, "SingleVerticalFence", Conveyor.Direction.North);
        }

        // Spawn corners

        BaseGameGrid.FillTilePrefab(xLeftBoundary, yTopBoundary, 1, 1, ItemGenerator.SingleTopLeftFencePath, "SingleTopLeftEndFence", Conveyor.Direction.North);
        BaseGameGrid.FillTilePrefab(xRightBoundary, yTopBoundary, 1, 1, ItemGenerator.SingleTopRightFencePath, "SingleTopRightEndFence", Conveyor.Direction.North);
        BaseGameGrid.FillTilePrefab(xLeftBoundary, yBotBoundary, 1, 1, ItemGenerator.SingleBotLeftFencePath, "SingleBotLeftEndFence", Conveyor.Direction.North);
        BaseGameGrid.FillTilePrefab(xRightBoundary, yBotBoundary, 1, 1, ItemGenerator.SingleBotRightFencePath, "SingleBotRightEndFence", Conveyor.Direction.North);

        if (spawnCustomerArea) SpawnCustomerArea(customerAreaSize, xLeftBoundary, xRightBoundary, yTopBoundary);
    }

    /** Spaws the customer area. */
    private void SpawnCustomerArea(int height, int xLeftBoundary, int xRightBoundary, int yTopBoundary)
    {
        //Spawn wood floor area
        for(int i = yTopBoundary - 1; i > yTopBoundary - 1 - height; i--)
        {
            for(int z = xLeftBoundary + 1; z < xRightBoundary; z++)
            {
                BaseGameGrid.CreateTile(z, i, woodFloor);
            }
        }

        //Spawn fence at bottom of wood area






    }
}
