using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class GameTile : MonoBehaviour
{
    /**The X position of this GameTile in its Grid. */
    public int X { get; private set; }

    /**The Y position of this GameTile in its Grid. */
    public int Y { get; private set; }

    /**The GameObject this tile holds. */
    public GameObject objectHolding; //Not a property so we can see it in the inspector.

    /**The GameGrid this Tile is in. */
    public GameObject[,] GridIn { get; set; }

    /**True if this tile has been created and placed in a GridArray.*/
    public bool TileSetUp { get; private set; }


    public void OnMouseOver()
    {
    }


    /**Sets the X and Y positions of this tile. 
     * x >= 0
     * y >= 0
     */
    public void SetTilePosition(int x, int y)
    {
        Assert.IsTrue(x >= 0 && y >= 0);
        if (!TileSetUp)
        {
            X = x;
            Y = y;
            TileSetUp = true;
        }
    }

    /**Puts a GameObject on this tile. */
    public void AddTileObject(GameObject objectToAdd)
    {
        objectHolding = objectToAdd;
    }

    /**Removes the GameObject from this tile. */
    public void RemoveTileObject()
    {
        objectHolding = null;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }
}
