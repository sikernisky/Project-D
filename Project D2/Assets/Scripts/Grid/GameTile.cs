using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class GameTile
{
    /**The X coordinate of this GameTile in its Grid. */
    public int X { get; private set; }

    /**The X world position of this GameTile in its Grid. */
    public float WorldX { get; private set; }

    /**The Y world position of this GameTile in its Grid. */
    public float WorldY { get; private set; }

    /**The Y coordinate of this GameTile in its Grid. */
    public int Y { get; private set; }

    /**The GameObject this tile holds. */
    public GameObject objectHolding;

    /**The GameGrid this Tile is in. */
    public GameGrid GridIn { get; private set; }

    /**True if this tile has been created and placed in a GridArray.*/
    public bool TileSetUp { get; private set; }

    private SpriteRenderer tileSpriteRenderer;

    public GameTile(int x, int y, float worldX, float worldY, GameGrid grid)
    {
        X = x;
        WorldX = worldX;

        Y = y;
        WorldY = worldY;

        GridIn = grid;
    }

    public void foo()
    {
    
    }

    /**Sets the X and Y positions of this tile. 
     * x >= 0
     * y >= 0
     */

    /**Puts a GameObject on this tile. */
    public void AddTileObject(GameObject objectToAdd)
    {
        objectHolding = objectToAdd;
    }

    /**Fills this GameTile with an object. */
    public void FillTile(MoveableScriptable itemToFill)
    {
        Debug.Log(itemToFill.itemClassName);
        Sprite test = itemToFill.basePlacedSprite;
        if (!TileSetUp) CreateRealTile(itemToFill.basePlacedSprite);
        else tileSpriteRenderer.sprite = itemToFill.basePlacedSprite;
    }

    /**Removes the GameObject from this tile. */
    public void RemoveTileObject()
    {
        objectHolding = null;
    }

    /**Returns a string representation of this GameTile.
     * "(X,Y)" */
    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public void CreateRealTile(Sprite spriteToSet)
    {
        GameObject newTile = new GameObject("NewTile");
        objectHolding = newTile;

        Transform newTileTransform = newTile.transform;
        newTileTransform.localPosition = new Vector2(WorldX, WorldY);
        newTileTransform.localScale = new Vector2(GridIn.CellSize, GridIn.CellSize);
        newTileTransform.SetParent(GridIn.ParentGameObject);

        newTile.AddComponent<SpriteRenderer>().sprite = spriteToSet;
        tileSpriteRenderer = objectHolding.GetComponent<SpriteRenderer>();
        tileSpriteRenderer.sortingOrder = GridIn.SortingOrder;

        newTile.AddComponent<BoxCollider2D>().size = new Vector2(GridIn.CellSize, GridIn.CellSize);

        TileSetUp = true;
    }
}
