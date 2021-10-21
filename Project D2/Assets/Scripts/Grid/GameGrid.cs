using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameGrid {

    /**NOTE: Code inspired by CodeMonkey on YouTube. */

    /**The width of this Grid (x-axis). */
    public int Width { get; private set; }

    /**The height of this Grid (y-axis). */
    public int Height { get; private set; }

    /**The height and width of each tile in this grid. */
    public float CellSize { get; private set; }

    /**A 2D array containing each tile in this Grid. */
    public GameObject[,] GridArray { get; private set; }

    /**An array of images that a tile in this grid might host. */
    public Sprite[] TileImages { get; set; }

    /**Creates an object of type GameGrid.
     * height: number of tiles on the y-axis. > 0
     * width: number of tiles on the x-axis. > 0
     * cellSize: the x by y size of each tile. > 0
     * tileImages: an array of Sprites, length > 0
     * parent: the parent of all the tiles, not null
     */
    public GameGrid(int width, int height, float cellSize, Sprite[] tileImages, Transform parent)
    {
        Assert.IsTrue(width > 0, "Width must be greater than zero.");
        Assert.IsTrue(height > 0, "Height must be greater than zero.");
        Assert.IsTrue(cellSize > 0, "CellSize must be greater than zero.");
        Assert.IsTrue(tileImages.Length > 0, "Length of TileImages must be greater than zero.");
        Assert.IsNotNull("parent cannot be null.");

        Width = width;
        Height = height;
        CellSize = cellSize;
        TileImages = tileImages;
        GridArray = new GameObject[width, height];

        for(int i = 0; i < GridArray.GetLength(0); i++)
        {
            for(int z = 0; z < GridArray.GetLength(1); z++)
            {
                GameObject newTile = new GameObject("newTile");
                Transform newTileTransform = newTile.transform;
                newTileTransform.localPosition = new Vector3(GetWorldPosition(i, z).x -1f, GetWorldPosition(i,z).y - 1f);
                newTileTransform.localScale = new Vector2(cellSize, cellSize);
                newTile.AddComponent<SpriteRenderer>().sprite = GetRandomFloorTile();
                newTile.AddComponent<GameTile>().SetTilePosition(i, z);
                newTile.GetComponent<GameTile>().GridIn = GridArray;
                newTile.GetComponent<Renderer>().sortingOrder = 4;
                newTile.AddComponent<BoxCollider2D>().size = new Vector2(i, z);
                newTileTransform.SetParent(parent);
                GridArray[i, z] = newTile;
            }
        }

        Camera.main.transform.localPosition = GetCenterOfGrid();

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
    private Sprite GetRandomFloorTile()
    {
        return TileImages[Random.Range(0, TileImages.Length - 1)];
    }


}
