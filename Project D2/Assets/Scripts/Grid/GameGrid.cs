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

        for (int i = 0; i < GridArray.GetLength(0); i++)
        {
            for (int z = 0; z < GridArray.GetLength(1); z++)
            {
                Vector3 worldPos = GetWorldPosition(i, z);
                GridArray[i, z] = new GameTile(i, z, worldPos.x, worldPos.y, this);
            }
        }

        Camera.main.transform.localPosition = GetCenterOfGrid();

    }

    public void FillAllTiles(Sprite[] imagesToFillWith)
    {
        foreach (GameTile tile in GridArray)
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
        foreach (GameTile tile in GridArray)
        {
            if (coordinates.x == tile.X && coordinates.y == tile.Y) return tile;
        }
        Debug.Log("Tile is not in grid.");
        return null;
    }

    /**Fills the tile at coordinates.x, coordinates.y with itemToAdd's class representation.
     * Precondition: itemToAdd must inherit from or be a MoveableScriptable. */
    public void FillTileInGrid(Vector2 proposedLocation, string itemToAdd, bool addClassComponent = true)
    {
        if (!CanPlaceTile(proposedLocation, new Vector2(1, 1))) return; // Make sure tile can be placed.
        GameTile tileSelected = GetTileFromGrid(proposedLocation);
        if (tileSelected == null) return; // Tile wasn't in the grid.
        ItemScriptable itemToAddScriptable = ItemGenerator.GetScriptableObject(itemToAdd);
        tileSelected.FillTile(itemToAddScriptable, addClassComponent);
    }


    /**Fills multiple tiles in the grid with the same item. 
     * 
     * Example: Suppose a grid below has [ ] empty and [x] filled.
     * 
     *   0    1    2    3    4    5    6    7
     *0 [x]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]
     *1 [ ]  [ ]  [ ]  [x]  [ ]  [ ]  [ ]  [ ]
     *2 [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]
     *3 [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]
     *4 [ ]  [ ]  [x]  [ ]  [ ]  [ ]  [ ]  [ ]
     *5 [ ]  [x]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]
     *6 [ ]  [ ]  [ ]  [ ]  [ ]  [x]  [ ]  [ ]
     *7 [ ]  [ ]  [ ]  [ ]  [ ]  [x]  [ ]  [ ]
     * 
     * Then, placing a 4x4 item y at (4,2) updates the grid from top left to bottom right:
     * 
     *   0    1    2    3    4    5    6    7
     *0 [x]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]
     *1 [ ]  [ ]  [ ]  [x]  [ ]  [ ]  [ ]  [ ]
     *2 [ ]  [ ]  [ ]  [ ]  [y]  [y]  [y]  [y]
     *3 [ ]  [ ]  [ ]  [ ]  [y]  [y]  [y]  [y]
     *4 [ ]  [ ]  [x]  [ ]  [y]  [y]  [y]  [y]
     *5 [ ]  [x]  [ ]  [ ]  [y]  [y]  [y]  [y]
     *6 [ ]  [ ]  [ ]  [ ]  [ ]  [x]  [ ]  [ ]
     *7 [ ]  [ ]  [ ]  [ ]  [ ]  [x]  [ ]  [ ]
     * 
     * Only the tile at (4,2) actually has its SpriteRenderer set. We increase its size.
     * --> For each expansion, increase its x position by 1 and decrease its y position by 1.
     *  --> Here, we are implementing a 4x4 tile. Increase x pos by 4 and decrease y pos by 4.
     *  --> Multiply x scale by 4 and y scale by 4.
     * 
     * Parameter activeCoordinates indicates which tiles in the area should be responsive. 
     * Suppose activeCoordinates = [(5,5) , (6,5)].
     * 
     * Under the hood, our grid has [x] occupied, [y] occupied and not interactable, [Y] interactable with added item.
     * 
     *   0    1    2    3    4    5    6    7
     *0 [x]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]  [ ]
     *1 [ ]  [ ]  [ ]  [x]  [ ]  [ ]  [ ]  [ ]
     *2 [ ]  [ ]  [ ]  [ ]  [y]  [y]  [y]  [y]
     *3 [ ]  [ ]  [ ]  [ ]  [y]  [y]  [y]  [y]
     *4 [ ]  [ ]  [x]  [ ]  [y]  [y]  [y]  [y]
     *5 [ ]  [x]  [ ]  [ ]  [y]  [Y]  [Y]  [y]
     *6 [ ]  [ ]  [ ]  [ ]  [ ]  [x]  [ ]  [ ]
     *7 [ ]  [ ]  [ ]  [ ]  [ ]  [x]  [ ]  [ ]     
     * 
     * 
     * Preconditions:
     * 
     *      coordinates: a Vector2 with a tile that exists in this grid.
     *      size: the size of this mxn tile. must be within the grid.
     *      activeCoordinates: the tiles in this mxn tile that are responsive.
     *      itemToAdd: must inherit from a Moveables scriptable.
     */
    public void FillTileInGrid(Vector2 proposedLocation, Vector2 tileSize, Vector2[] activeCoordinates, string itemToAdd, bool scaleBySize = false, bool addClassComponent = false)
    {
        if (!CanPlaceTile(proposedLocation, tileSize)) {
            return;
        } // Make sure tile can be placed.

        AssignActiveTiles(activeCoordinates, itemToAdd); // Assign the active tiles. 

        FillTileInGrid(proposedLocation, itemToAdd, addClassComponent); // Fill the sprite tile; don't assign it active.

        GameTile spriteTile = GetTileFromGrid(proposedLocation);
        if(scaleBySize) spriteTile.objectHolding.transform.localScale = new Vector2(tileSize.x * CellSize, tileSize.y * CellSize);

        Vector2 currentTilePosition = spriteTile.objectHolding.transform.localPosition;
        spriteTile.objectHolding.transform.localPosition = GetLargeTilePosition(currentTilePosition, tileSize); // Set position
    }

    /**Assigns each tile with a coordinate in activeCoordinates the Class itemToAdd. */
    private void AssignActiveTiles(Vector2[] activeCoordinates, string itemToAdd)
    {
        foreach (Vector2 coordinate in activeCoordinates)
        {
            GameTile coordinateTile = GetTileFromGrid(coordinate);
            if (coordinateTile == null)
            {
                Debug.Log("The coordinate of which you are trying to set active is not in the Grid.");
                return;
            }
        }

        foreach (Vector2 coordinate in activeCoordinates)
        {
            GameTile coordinateTile = GetTileFromGrid(coordinate);
            coordinateTile.objectHolding.AddComponent(ItemGenerator.GetClassFromString(itemToAdd));
        }

    }

    /**Returns True if this tile can be placed at proposedLocation; false otherwise 
         * Precondition: tileSize is the raw positions for a grid: (2,2) means it takes up FOUR GameTiles 
         * Precondition: proposedLocation is the raw location in a grid: (2,2) it is placed at (2,2). 
         */
    private bool CanPlaceTile(Vector2 proposedLocation, Vector2 tileSize)
    {
        bool result = true;
        if (proposedLocation.x + tileSize.x - 1 >= Width || proposedLocation.x < 0) result = false;
        if (proposedLocation.y - tileSize.y  + 1< 0 || proposedLocation.y >= Height) result = false;
        if (!result) Debug.Log("Tile of raw size " + tileSize.x + " by " + tileSize.y + " cannot fit at " + proposedLocation); 
        return result;
    }

    /**Gets the correct position for a tile larger than 1x1. 
     * Precondition: tileSize is the raw positions for a grid: (2,2) means it takes up FOUR GameTiles 
     * Precondition: proposedLocation is the raw location in a grid: (2,2) it is placed at (2,2). 
     */
    private Vector2 GetLargeTilePosition(Vector2 proposedLocation, Vector2 tileSize)
    {
        float floatErrorOffset;
        if (tileSize.x == 3) floatErrorOffset = .123f;
        else floatErrorOffset = 0f;

        Vector2 scaledTileSize = new Vector2(tileSize.x * CellSize, tileSize.y * CellSize);
        return new Vector2(proposedLocation.x + floatErrorOffset + (scaledTileSize.x - 2)/CellSize, proposedLocation.y - (scaledTileSize.y -2)/CellSize);
    }


}
