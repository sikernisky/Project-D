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

    public SpriteRenderer tileSpriteRenderer;

    public GameTile(int x, int y, float worldX, float worldY, GameGrid grid)
    {
        X = x;
        WorldX = worldX;

        Y = y;
        WorldY = worldY;

        GridIn = grid;
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


    /** Creates the Tile in the scene. More specifically:
     *  Creates the GameObject
     *  Adds a ListenerTile component to detect clicks, hovers, and placements
     *  Sets the position and scale of the GameObject
     *  Sets the parent of the GameObject
     *  Adds a SpriteRenderer component and sets its sprite field to parameter spriteToSet
     *  Updates its sorting order according to its Grid
     *  Adds a BoxCollider2D component and sets its size field according to its Grid's cellsize
     */
    public void CreateRealTile(Sprite spriteToSet)
    {
        GameObject newTile = new GameObject("NewTile");
        objectHolding = newTile;

        Transform newTileTransform = newTile.transform;
        newTileTransform.localPosition = new Vector2(WorldX, WorldY);
        newTileTransform.localScale = new Vector2(GridIn.CellSize +.01f, GridIn.CellSize + .01f);
        newTileTransform.SetParent(GridIn.ParentGameObject);

        newTile.AddComponent<SpriteRenderer>().sprite = spriteToSet;
        tileSpriteRenderer = objectHolding.GetComponent<SpriteRenderer>();
        tileSpriteRenderer.sortingOrder = GridIn.SortingOrder;

        TileSetUp = true;
    }
}
