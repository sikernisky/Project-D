using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerTile : MonoBehaviour
{
    /**The X Y world coordinates of this ListerTile. Its localPosition. */
    public Vector2 Coordinates { get; private set; }

    /**The GameTile this ListerTile corresponds to. */
    public GameTile tileGameTile { get; set; }

    /**The FluidItem the mouse is hovering over. */
    public static FluidItem MoveablesItemHovering { get; private set; }


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

    private void OnMouseOver()
    {
        AssignMoveablesItemHovering();
    }

    private void AssignMoveablesItemHovering()
    {
        if (GameGridMaster.MoveablesGameGrid.GetTileFromGrid(Coordinates) != null)
        {
            if (GameGridMaster.MoveablesGameGrid.GetTileFromGrid(Coordinates).objectHolding != null)
            {
                if (GameGridMaster.MoveablesGameGrid.GetTileFromGrid(Coordinates).objectHolding.GetComponent<FluidItem>() != null)
                {
                    MoveablesItemHovering = GameGridMaster.MoveablesGameGrid.GetTileFromGrid(Coordinates).objectHolding.GetComponent<FluidItem>();
                }
                else MoveablesItemHovering = null;
            }
            else MoveablesItemHovering = null;
        }
        else MoveablesItemHovering = null;
    }
}
