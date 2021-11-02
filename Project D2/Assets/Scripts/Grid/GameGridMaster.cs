using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridMaster : MonoBehaviour
{
    /**The cell size of created GameGrids. */
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


    /**Creates a GameGrid with width width and height height.
     (1) width: the width of this grid.
     (2) height: the height of this grid. */
    private GameGrid CreateGroundTilemap(int width, int height)
    {
        GameObject GroundTilemapParent = new GameObject("GroundTilemapMaster");
        GameGrid grid = new GameGrid(width, height, cellSize, GroundTilemapParent.transform);
        grid.FillAllTiles(baseTileArray);
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
                GridWidth = 30;
                GridHeight = 30;
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
                //Fill a tile at (13,15) with size (3,3) and set its next tile to (14,20)
                MoveablesGameGrid.FillTile(13,19,3,2,ItemGenerator.PlateStationPath, "PlateStation", north, 14, 20);
                MoveablesGameGrid.FillTile(14,20, 1,1, ItemGenerator.DeluxeConveyorPath,"DeluxeConveyor", west);
                MoveablesGameGrid.FillTile(13,20,1,1, ItemGenerator.DefaultConveyorPath,"DefaultConveyor", west);
                MoveablesGameGrid.FillTile(12, 20, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", west);
                MoveablesGameGrid.FillTile(11, 20, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTile(11, 21, 1, 1, ItemGenerator.DefaultConveyorPath, "DeluxeConveyor", north);
                MoveablesGameGrid.FillTile(11, 22, 1, 1, ItemGenerator.DefaultConveyorPath, "DeluxeConveyor", north);
                MoveablesGameGrid.FillTile(11, 23, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTile(11, 24, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", east);
                MoveablesGameGrid.FillTile(12, 24, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", east);
                MoveablesGameGrid.FillTile(13, 24, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", east);
                MoveablesGameGrid.FillTile(14, 24, 1, 1, ItemGenerator.DefaultConveyorPath, "DeluxeConveyor", north);
                MoveablesGameGrid.FillTile(14,24, 1,1, ItemGenerator.DefaultConveyorPath,"DefaultConveyor", north);
                MoveablesGameGrid.FillTile(14,25,1,1,ItemGenerator.DefaultConveyorPath,"DefaultConveyor", north, 13, 26);
    
                MoveablesGameGrid.FillTile(13,26, 3,2, ItemGenerator.ServeStationPath,"ServeStation", north, 14, 27);
                MoveablesGameGrid.FillTile(14, 27, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTile(14, 28, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);
                MoveablesGameGrid.FillTile(14, 29, 1, 1, ItemGenerator.DefaultConveyorPath, "DefaultConveyor", north);



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
}
