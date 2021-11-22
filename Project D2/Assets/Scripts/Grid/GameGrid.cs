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
     (1) height: number of tiles on the y-axis. > 0
     (2) width: number of tiles on the x-axis. > 0
     (3) cellSize: the x by y size of each tile. > 0
     (4) tileImages: an array of Sprites, can be null
     (5) parent: the parent of all the tiles, not null
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

    /**Fills each GameTile's SpriteRenderer in GridArray with a random sprite in imagesToFillWith. 
     (1) imagesToFillWith: an array of sprites used to paint the grid.*/
    public void CreateAllTiles(Sprite[] imagesToFillWith)
    {
        foreach (GameTile tile in GridArray)
        {
            if (imagesToFillWith == null) tile.CreateRealTile(GetRandomTileSprite(null));
            else tile.CreateRealTile(GetRandomTileSprite(imagesToFillWith));
        }
    }

    public void SetAllTilesWalkable()
    {
        foreach(GameTile tile in GridArray)
        {
            tile.Walkable = true;
        }
    }

    public void CreateTile(int xPos, int yPos, Sprite image)
    {
        GameTile tile = GetTileFromGrid(new Vector2(xPos, yPos));
        if (image == null) tile.CreateRealTile(null);
        else tile.CreateRealTile(image);
    }

    /**Returns a Vector3 that describes the position of a tile based on its x pos, y pos, and cellsize. 
     (1) x: the x position of the tile 
     (2) y: the y position of the tile */

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

    /**Returns a random Sprite in TileImages.
     (1) sprites: an array of sprites used in random selection.*/

    private Sprite GetRandomTileSprite(Sprite[] sprites)
    {
        if (sprites == null) return null;
        return sprites[Random.Range(0, sprites.Length - 1)];
    }

    /**Gets a GameTile from this GridArray by its coordinates. Returns null if not in the array.
     (1) coordinates: the grid coordinates of the desired tile. */
    public GameTile GetTileFromGrid(Vector2 coordinates)
    {
        foreach (GameTile tile in GridArray)
        {
            if (coordinates.x == tile.X && coordinates.y == tile.Y) return tile;
        }
        return null; // Tile is not in grid.
    }


    /**Fills one or more tiles in the grid with an Item. 
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
     * Then, placing a 4x4 item [y] at (4,2) updates the grid from top left to bottom right:
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
     * Only the tile at (4,2) actually has its SpriteRenderer set. We adjust its transform.position accordingly.
     * 
     * Precondition: prefabSource: links to a valid prefab.
  
     */
    public void FillTilePrefab(int xPos, int yPos, int xSize, int ySize, string prefabSource, string name, Conveyor.Direction direction, int nextXPos = -1, int nextYPos = -1)
    {

        Vector2 nextTilePos;

        if(nextXPos == -1 && nextYPos == -1)
        {
            if (direction == Conveyor.Direction.North) nextTilePos = new Vector2(xPos, yPos + 1);
            else if (direction == Conveyor.Direction.East) nextTilePos = new Vector2(xPos + 1, yPos);
            else if (direction == Conveyor.Direction.South) nextTilePos = new Vector2(xPos, yPos - 1);
            else nextTilePos = new Vector2(xPos - 1, yPos);
        }
        else nextTilePos = new Vector2(nextXPos, nextYPos);

        Vector2 proposedLocation = new Vector2(xPos, yPos);
        Vector2 tileSize = new Vector2(xSize, ySize);

        if (!CanPlaceTile(proposedLocation, tileSize)) return; // Do nothing if the tile is placed at an invalid spot.

        GameTile tileSelected = GetTileFromGrid(proposedLocation);
        if (tileSelected == null) return;
        GameObject actualTile = tileSelected.FillTile(prefabSource, GetTileFromGrid(nextTilePos), name, tileSize, direction);
        if (actualTile == null) Debug.Log("Tile: " + name + " did not spawn a real tile.");

        // We need declare tiles filled by a larger tile occupied
        if(tileSize.x > 1 || tileSize.y > 1) AssignActiveTiles(proposedLocation, tileSize, actualTile, name);
    }

    /**For each GameTile within a size.x by size.y tile placed at location, calls OccupyTile(name).
     * See the specification for FillTile() for more information on large tile placement.*/
    public void AssignActiveTiles(Vector2 location, Vector2 size, GameObject actualTile, string name)
    {
        int x = (int) size.x;
        int y = (int) size.y;

        for (int i = (int)location.x; i < location.x + x; i++)
        {
            for(int z = (int)location.y; z > location.y - y; z--)
            {
                GameTile tileSelected = GetTileFromGrid(new Vector2(i, z));
                tileSelected.OccupyTile(actualTile, name);
            }
        }



    }

    /**Returns True if this tile can be placed at proposedLocation; false otherwise. */
    private bool CanPlaceTile(Vector2 proposedLocation, Vector2 tileSize)
    {
        bool result = true;
        if (tileSize.x < 1 || tileSize.y < 1) result = false;
        if (proposedLocation.x + tileSize.x - 1 >= Width || proposedLocation.x < 0) result = false;
        if (proposedLocation.y - tileSize.y  + 1< 0 || proposedLocation.y >= Height) result = false;
        if (!result) Debug.Log("Tile of raw size " + tileSize.x + " by " + tileSize.y + " cannot fit at " + proposedLocation); 
        return result;
    }


}
