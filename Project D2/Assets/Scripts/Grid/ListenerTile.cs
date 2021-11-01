using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerTile : MonoBehaviour
{
    /**The X Y world coordinates of this ListerTile. Its localPosition. */
    public Vector2 Coordinates { get; private set; }

    /**The GameTile this ListerTile corresponds to. */
    public GameTile tileGameTile { get; set; }

    public void SetCoordinates()
    {
        Coordinates = new Vector2(tileGameTile.X, tileGameTile.Y);
    }

    /**Returns a string representation of this tile. */

    public override string ToString()
    {
        return tileGameTile.ToString();
    }

    private void Start()
    {
        gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1,1);
    }

    public void OnMouseDown()
    {

        GameTile clickedTile = GameGridMaster.MoveablesGameGrid.GetTileFromGrid(Coordinates);
        if (clickedTile.Occupied) Debug.Log(clickedTile.GameTileName);
        else Debug.Log("Not occupied.");

        //Debug.Log(tileGameTile.X + ", " + tileGameTile.Y);
    }
}
