using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

/// <summary>
/// Represents a Tile in a TileGrid. 
/// </summary>
public class Tile : MonoBehaviour
{

    /**The method SpawnTile() below MUST be called on a Tile script before adding it to a
    * GameObject. If this advice is voided, unexpected behaviour may occur. */

    ///<summary>In indexed order: This Tile's Northern, Eastern, Southern, and Western neighbors.</summary>
    private Tile[] neighbors = new Tile[4];

    ///<summary> The (x,y) position of this Tile in its TileGrid. </summary> 
    private Vector2Int coords;

    ///<summary> The unique ID of this Tile. </summary> 
    private string id;

    ///<summary>The unique ID of this Tile's <c>occupier</c>.</summary>
    private long structureID;

    ///<summary> The TileGrid this Tile lies in. </summary> 
    private TileGrid parentGrid;

    ///<summary> <c>true</c> if this Tile has been spawned by its parent TileGrid. </summary> 
    private bool spawned;

    ///<summary> <c>true</c> if this Tile is occupied.</summary> 
    private bool occupied;

    ///<summary> The Structure that occupies this Tile; <c>null</c> if it is unoccupied. </summary> 
    private Structure occupier;

    /// <summary> This <c>Tile</c>'s SpriteRenderer.</summary>
    private SpriteRenderer spriteRenderer;

    ///<summary> Spawns a Tile at <c>xPosition</c>, <c>yPosition</c>. Offically spawns the Tile.
    ///<br></br> <em>Precondition:</em>: <c>xPosition</c> and <c>yPosition</c> are valid coordinates.
    ///<br></br> <em>Precondition:</em>: <c>parent</c> is not <c>null</c> and contains this Tile. 
    ///<br></br> <em>Precondition:</em>: this Tile has not already been spawned. </summary> 
    public void SpawnTile(int xPosition, int yPosition, TileGrid parent)
    {
        Assert.IsTrue(xPosition >= 0 && yPosition >= 0, "Parameters xPosition and yPosition must be greater than zero.");
        Assert.IsNotNull(parent, "Parameter parent cannot be null.");
        Assert.IsFalse(spawned, "Parameter spawned must be false.");

        spawned = true;
        coords = new Vector2Int(xPosition, yPosition);
        parentGrid = parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MakeID();
    }
    
    /// <summary>Sets, in indexed order, this Tile's northern, eastern, southern, and western neighbors. 
    /// <br></br><em>Precondition</em>: <c>neighbors</c>[0..3] is north, east, south, and west.
    /// <br></br><em>Precondition</em>: <c>neighbors</c> is not null and has size 4. </summary>
    public void SetNeighbors(Tile[] neighbors)
    {
        Assert.IsNotNull(neighbors, "Parameter neighbors cannot be null.");
        Assert.IsTrue(neighbors.Length == 4, "Parameter neighbors must have length 4.");
        if (neighbors[0] != null) Assert.IsTrue(IsNeighbor(neighbors[0], Direction.North), "neighbors[0] must be null or a neighbor of this Tile.");
        if (neighbors[1] != null) Assert.IsTrue(IsNeighbor(neighbors[1], Direction.East), "neighbors[0] must be null or a neighbor of this Tile.");
        if (neighbors[2] != null) Assert.IsTrue(IsNeighbor(neighbors[2], Direction.South), "neighbors[0] must be null or a neighbor of this Tile.");
        if (neighbors[3] != null) Assert.IsTrue(IsNeighbor(neighbors[3], Direction.West), "neighbors[0] must be null or a neighbor of this Tile.");
        this.neighbors = neighbors;
    }

    /// <summary><strong>Returns:</strong> this Tile's northern neighbor, or <c>null</c> if none exists.</summary>
    public Tile NorthernNeighbor()
    {
        return neighbors[0];
    }

    /// <summary><strong>Returns:</strong> this Tile's eastern neighbor, or <c>null</c> if none exists.</summary>
    public Tile EasternNeighbor()
    {
        return neighbors[1];
    }

    /// <summary><strong>Returns:</strong> this Tile's southern neighbor, or <c>null</c> if none exists.</summary>
    public Tile SouthernNeighbor()
    {
        return neighbors[2];
    }

    /// <summary><strong>Returns:</strong> this Tile's western neighbor, or <c>null</c> if none exists.</summary>
    public Tile WesternNeighbor()
    {
        return neighbors[3];
    }

    /// <summary><strong>Returns:</strong> All of this Tile's neighbors.</summary>
    public Tile[] AllNeighbors()
    {
        return neighbors;
    }

    /// <summary><strong>Returns:</strong> <c>true</c> if <c>w</c> is a neighbor of this Tile in Direction <c>d</c>. </summary>
    private bool IsNeighbor(Tile w, Direction d) 
    {
        Vector2Int wCoords = w.Coordinates();
        if (d == Direction.North)
        {
            if (coords.y != wCoords.y - TileGrid.tileSize) return false;
            if (coords.x != wCoords.x) return false;
        }
        if(d == Direction.East)
        {
            if (coords.x != (wCoords.x - TileGrid.tileSize)) return false;
            if (coords.y != wCoords.y) return false;
        }
        if(d == Direction.South)
        {
            if (coords.y - TileGrid.tileSize != wCoords.y) return false;
            if (coords.x != wCoords.x) return false;
        }
        if(d == Direction.West)
        {
            if (coords.x - TileGrid.tileSize != wCoords.x) return false;
            if (coords.y != wCoords.y) return false;
        }

        return true;
    }

    ///<summary> Constructs this Tile's ID. </summary> 
    private void MakeID()
    {
        id = coords.x.ToString() + coords.y + parentGrid.Name();
    }

    ///<summary><strong> Returns:</strong> the unique ID of this Tile. </summary> 
    public string ID()
    {
        return id;
    }

    ///<summary> <strong>Returns:</strong> the (x, y) local position coordinates of this Tile. </summary> 
    public Vector2Int Coordinates()
    {
        return coords;
    }

    ///<summary> Occupies this Tile with <c>occupier</c>.
    ///<br></br> <em>Precondition:</em> this Tile is not already occupied.
    ///<br></br><em>Precondition:</em> <c>occupier</c> is not null.
    ///<br></br><em>Precondition:</em> <c>structureID</c> is 0.</summary>

    public void SetOccupied(Structure occupier, long ID)
    {
        Assert.IsNotNull(occupier, "Parameter occupier cannot be null.");
        Assert.IsFalse(IsOccupied(), "You may not place on tile " + ToString() + " because it is occupied.");
        Assert.IsNull(this.occupier, "You may not place on tile " + ToString() + " because its occupier is not null.");
        Assert.IsTrue(structureID == 0, "You may not place on tile " + ToString() + " because its structureID is not 0.");
        occupied = true;
        this.occupier = occupier;
        this.occupier.AddOccupyingTile(this);
        structureID = ID;
    }

    ///<summary> Removes anything occupying this Tile.
    /// <br></br> <em>Precondition:</em> this Tile is occupied.</summary> 
    public void Empty()
    {
        Assert.IsTrue(IsOccupied(), "You may empty tile " + ToString() + " because it is already unoccupied.");
        Assert.IsTrue(StructureID() != 0, "You may empty tile " + ToString() + " because its ID is 0.");
        Assert.IsNotNull(occupier, "You may not empty tile " + ToString() + " because its occupier is null.");

        occupied = false;
        structureID = 0;
        occupier.RemOccupyingTile(this);
        occupier.DestroyAttachedObject();
        occupier = null;
    }

    ///<summary> <strong>Returns:</strong> Tile in string format: "(<c>x, y</c>) in TileGrid: <c>parent</c> <c>name</c>." </summary> 
    public override string ToString()
    {
        return "(" + (int)coords.x + "," + (int)coords.y + ") in TileGrid: " + parentGrid.Name() + "."; 
    }

    ///<summary> <strong>Returns:</strong> <c>true</c> if this Tile is occupied. </summary> 
    public bool IsOccupied()
    {
        return occupied;
    }

    /// <summary> Sets this <c>Tile</c>'s Sprite color.</summary>
    public void SetColor(Color32 c)
    {
        spriteRenderer.color = c;
    }

    /// <summary><strong>Returns:</strong> The unique ID of this Tile's occupying Structure.
    /// <br></br><em>Precondition:</em> this Tile is occupied.</summary>
    public long StructureID()
    {
        Assert.IsTrue(IsOccupied(), "You may not retrieve the StructureID of this Tile because it is unoccupied.");
        return structureID;
    }

    /// <summary><strong>Returns:</strong> the Structure on this Tile, or <c>null</c> if it doesn't have one.</summary>
    public Structure Structure()
    {
        return occupier;
    }

    /// <summary>Sets this Tile's <c>renderer</c>'s Sprite as <c>s</c>.
    /// <br></br><em>Precondition:</em> <c>s</c> is not <c>null</c>.</summary>
    public void SetTileSprite(Sprite s)
    {
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        spriteRenderer.sprite = s;
    }

    private void OnMouseOver()
    {
        TileGrid.tileHovering = this;
    }

    private void OnMouseExit()
    {
        TileGrid.tileHovering = null;
    }

    private void OnMouseDown()
    {
        Connector c = Connector.ConnectorDragging;
        if (Structure() != null) Structure().OnClick(this);
        else if (c != null && c.IsDraggingLine()) c.DropLine();
        TileGrid.lastClickedTile = this;
    }
}
