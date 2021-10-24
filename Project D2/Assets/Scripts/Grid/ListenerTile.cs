using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerTile : MonoBehaviour
{
    /**The X Y world coordinates of this ListerTile. Its localPosition. */
    public Vector2 Coordinates { get; private set; }

    /**The GameTile this ListerTile corresponds to. */
    public GameTile tileGameTile { get; set; }

    public void SetCoordinates(Vector2 proposedCoordinates)
    {
        Coordinates = proposedCoordinates;
    }

    /**Returns a string representation of this tile. */

    public override string ToString()
    {
        return tileGameTile.ToString();
    }


    public void OnMouseDown()
    {
        //int clickedTilePosX = gameObject.GetComponent<ListenerTile>().tileGameTile.X;
        //int clickedTilePosY = gameObject.GetComponent<ListenerTile>().tileGameTile.Y;

        //GameTile clickedTile = GameGridMaster.BaseGameGrid.GetTileFromGrid(new Vector2(clickedTilePosX, clickedTilePosY));
        //clickedTile.tileSpriteRenderer.color = new Color32(255, 0, 0, 255);
    }
}
