using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameGrid {

    /**NOTE: Code inspired by CodeMonkey on YouTube. */

    //The GridArray holds a 2D array of GameTile objects.
    //They aren't gameobjects quite yet. To bring it to life, we create a gameobject at its coords.
    

    /**The width of this Grid (x-axis). */
    public int Width { get; private set; }

    /**The height of this Grid (y-axis). */
    public int Height { get; private set; }

    /**The Sorting Order of this grid.*/
    public int SortingOrder { get; private set; }

    /**The height and width of each tile in this grid. */
    public float CellSize { get; private set; }

    /**A 2D array containing each tile in this Grid. */
    public GameTile[,] GridArray { get; private set; }

    /**The parent of this grid. */
    public Transform ParentGameObject { get; set; }

    /**Creates an object of type GameGrid.
     * height: number of tiles on the y-axis. > 0
     * width: number of tiles on the x-axis. > 0
     * cellSize: the x by y size of each tile. > 0
     * tileImages: an array of Sprites, can be null
     * parent: the parent of all the tiles, not null
     */
    public GameGrid(int width, int height, float cellSize, Transform parent, int sortingOrder = 0)
    {
        Assert.IsTrue(width > 0, "Width must be greater than zero.");
        Assert.IsTrue(height > 0, "Height must be greater than zero.");
        Assert.IsTrue(cellSize > 0, "CellSize must be greater than zero.");
        Assert.IsNotNull(parent, "parent cannot be null.");

        Width = width;
        Height = height;
        SortingOrder = sortingOrder;
        CellSize = cellSize;
        ParentGameObject = parent;
        GridArray = new GameTile[width, height];

        for(int i = 0; i < GridArray.GetLength(0); i++)
        {
            for(int z = 0; z < GridArray.GetLength(1); z++)
            {
                Vector3 worldPos = GetWorldPosition(i, z);
                Debug.Log("WorldX is " + worldPos.x + " and WorldY is " + worldPos.y);
                GridArray[i, z] = new GameTile(i, z, worldPos.x, worldPos.y, this);
            }
        }

        Camera.main.transform.localPosition = GetCenterOfGrid();

    }

    public void FillAllTiles(Sprite[] imagesToFillWith)
    {
        foreach(GameTile tile in GridArray)
        {
            if (imagesToFillWith == null) tile.CreateRealTile(GetRandomTileSprite(null));
            else tile.CreateRealTile(GetRandomTileSprite(imagesToFillWith));
        }
    }

    /**Returns a Vector3 that describes the position of a tile based on its 
     * x pos, y pos, and cellsize. */
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize;
    }

    /**Returns a Vector3 that describes the position of the center of the grid. */
    public Vector3 GetCenterOfGrid()
    {
        float xPos = (Width * CellSize) / 2;
        float yPos = (Height * CellSize) / 2;
        return new Vector3(xPos, yPos, -10);
    }

    /**Returns a random Sprite in TileImages. */
    private Sprite GetRandomTileSprite(Sprite[] sprites)
    {
        if (sprites == null) return null;
        return sprites[Random.Range(0, sprites.Length - 1)];
    }

    /**Gets a GameTile from this GridArray by its coordinates. Returns null if not in the array.*/
    public GameTile GetTileFromGrid(Vector2 coordinates)
    {
        foreach(GameTile tile in GridArray)
        {
            if (coordinates.x == tile.X && coordinates.y == tile.Y) return tile;
        }
        Debug.Log("Tile is not in grid.");
        return null;
    }

    /**Fills the tile at coordinates.x, coordinates.y with itemToAdd's class representation.
     * Precondition: itemToAdd must inherit from or be a MoveableScriptable. */
    public void FillTileInGrid(Vector2 coordinates, string itemToAdd)
    {
        GameTile tileSelected = GetTileFromGrid(coordinates);
        if (tileSelected == null) return; // Tile wasn't in the grid.
        ItemScriptable itemToAddScriptable = FoodGenerator.GetScriptableObject(itemToAdd);
        tileSelected.FillTile((MoveableScriptable)itemToAddScriptable);
    }


}
