using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Represents a grid of Tile objects.
/// </summary>
public class TileGrid
{

    ///<summary> The Tile the user is hovering over; <c>null</c> if the mouse is not over a Tile. </summary>
    public static Tile tileHovering;

    /// <summary>The Tile last clicked on by the player; <c>null</c> if the player has not clicked on a Tile.</summary>
    public static Tile lastClickedTile;

    ///<summary> Size of each Tile. </summary>
    public static readonly int tileSize = 2;

    /// <summary> Size of the padding between each tile. </summary>
    public static readonly float padding = .01f;

    ///<summary> Name of this TileGrid. </summary>
    private string gridName;

    ///<summary> Width and height, in number of Tiles, of this TileGrid. </summary>
    private readonly int gridSize;

    /// <summary> The Tiles highlighted by attempting to place a placeable. </summary>
    private HashSet<Tile> highligtedTiles;

    ///<summary> The set of all Tiles in this TileGrid. </summary>
    private readonly HashSet<Tile> tiles = new HashSet<Tile>();

    /// <summary>The set of unique Structure IDs. </summary>
    private static HashSet<long> StructureIDs = new HashSet<long>();

    ///<summary> <strong>Constructor</strong>: A TileGrid with: 
    ///<br></br><c>name</c>: The name of this TileGrid.
    ///<br></br><c>size</c>: The width and height, in Tiles, of this TileGrid.
    ///<br></br><c>parent</c>: The parent transform of all Tiles in this TileGrid.
    ///<br></br><c>tSprites</c>: Possible Sprites a Tile may be assigned.
    ///<br></br><c>preSpawns</c>: Each IPlaceable key is spawned at all of its Vector2Int values.
    ///<br></br><c>customerHeight</c>: The height of this TileGrid's customer area.
    ///<br></br><em>Precondition:</em> <c>name</c> is not null.
    ///<br></br><em>Precondition:</em> <c>size</c> must be even and > 0.
    ///<br></br><em>Precondition:</em> <c>parent</c> is not null.
    ///<br></br><em>Precondition:</em> <c>tSprites</c> is not null.
    ///<br></br><em>Precondition:</em> <c>preSpawns</c> and its keys and values are not null.
    ///<br></br><em>Precondition:</em> <c>customerHeight</c> is greater than zero and less than or equal to <c>size</c>.
    /// </summary>
    public TileGrid(string name, int size, Transform parent, List<Sprite> tSprites, Dictionary<IPlaceable, List<Vector2Int>> preSpawns, int customerHeight)
    {
        Assert.IsNotNull(name, "Parameter name cannot be null.");
        Assert.IsTrue(size % 2 == 0 && size > 1, "Parameter size must be even and > 0.");
        Assert.IsNotNull(parent, "Parameter parent cannot be null.");
        Assert.IsNotNull(name, "Parameter tSprites cannot be null.");
        Assert.IsFalse(IsPreSpawnsNull(preSpawns), "Parameter preSpawns and its keys and values cannot be null.");
        Assert.IsTrue(customerHeight > 0 && customerHeight <= size, "Customer area height must be > 0 and <= size.");
        gridName = name;
        gridSize = size;
        highligtedTiles = new HashSet<Tile>();
        ConstructGrid(parent, tSprites);
        SpawnStructures(preSpawns);
        ConstructCustomerArea(customerHeight);
    }

    ///<summary> Constructs a TileGrid of Tile objects under <c>parent</c> in the Hierarchy.
    ///Each tile is assigned a random sprite from <c>tSprites</c>. </summary>
    private void ConstructGrid(Transform parent, List<Sprite> tSprites)
    {
        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                Vector2 spawnedPos = new Vector2(x * tileSize, y * tileSize);
                Vector2 sizeVector = new Vector2(tileSize + padding, tileSize + padding);

                GameObject tob = new GameObject();
                tob.transform.SetParent(parent);
                tob.transform.localPosition = spawnedPos;
                tob.transform.localScale = sizeVector;

                BoxCollider2D col = tob.AddComponent<BoxCollider2D>();
                col.size = new Vector2(1, 1);

                SpriteRenderer spr = tob.AddComponent<SpriteRenderer>();
                if(tSprites != null) spr.sprite = tSprites[UnityEngine.Random.Range(0, tSprites.Count - 1)];

                Tile t = tob.AddComponent<Tile>();
                t.SpawnTile(x * tileSize, y * tileSize, this);
                tiles.Add(t);
            }
        }
        SetTileNeighbors();
    }

    /// <summary>Creates a customer area <c>height</c> Tiles tall starting from the top of this TileGrid.
    /// <br></br>This area crosses the entire grid horizontally.
    /// <br></br><em>Precondition:</em> <c>height</c> is greater than 0 and less or equal to than grid size.</summary>
    private void ConstructCustomerArea(int height)
    {
        Assert.IsTrue(height > 0 && height <= gridSize, "Customer area must be larger than 0 and less than the grid's size.");
        for(int x = 0; x < gridSize * tileSize; x += tileSize)
        {
            int yStart = (gridSize * tileSize) - tileSize;
            for (int y = yStart; y > yStart - height * tileSize; y-= tileSize)
            {
                Tile t = TileByLoc(new Vector2Int(x, y));
                t.SetAsCustomerTile(Resources.Load<Sprite>("Tiles/WoodFloor"));
            }
        }
    }
    
    /// <summary><strong>Returns:</strong> true if <c>preSpawns</c> and each of its keys and values is not <c>null</c>.</summary>
    private bool IsPreSpawnsNull(Dictionary<IPlaceable, List<Vector2Int>> preSpawns)
    {
        if (preSpawns == null) return true;
        foreach(KeyValuePair<IPlaceable, List<Vector2Int>> pair in preSpawns)
        {
            if (pair.Key == null) return true;
            if (pair.Value == null) return true;
        }
        return false;
    }

    /// <summary>For each IPlaceable key in <c>preSpawns:</c>
    /// <br></br>Spawns its associated Structure at all of the Vector2Int positions in its value.
    /// <br></br><em>Precondition:</em> preSpawns and its keys and values are not <c>null</c>.</summary>
    private void SpawnStructures(Dictionary<IPlaceable, List<Vector2Int>> preSpawns)
    {
        Assert.IsFalse(IsPreSpawnsNull(preSpawns), "Parameter preSpawns and its keys and values cannot be null.");
        foreach (KeyValuePair<IPlaceable,List<Vector2Int>> pair in preSpawns)
        {
            IPlaceable p = pair.Key;
            List<Vector2Int> positions = pair.Value;
            foreach(Vector2Int v in positions)
            {
                Place(p, TileByLoc(v));
            }
        }
    }

    ///<summary> <strong>Returns:</strong> the name of this TileGrid. It will never be null.</summary>
    public string Name()
    {
        return gridName;
    }

    ///<summary><strong>Returns: </strong> <c>true</c> and places <c>placeable</c> on Tile <c>t</c> if it is possible.
    ///<br></br><em>Precondition:</em> <c>placeable.PrefabObject()</c> is not <c>null</c> and has a Structure component.</summary>
    public bool Place(IPlaceable placeable, Tile t)
    {
        if (!CanPlace(placeable, t)) return false;

        HashSet<Tile> affected = AffectedTiles(placeable, t);

        GameObject ob = placeable.PrefabObject();

        Assert.IsNotNull(ob, "Variable ob, gathered from placeable.PrefabObject(), cannot be null.");
        Assert.IsNotNull(ob.GetComponent<Structure>(), "Variable ob, gathered from placeable.PrefabObject(), must have a Structure component.");

        GameObject cob = GameObject.Instantiate(ob);
        cob.transform.position = new Vector3(t.Coordinates().x + placeable.Bounds().x - 1, t.Coordinates().y - placeable.Bounds().y + 1, 10);
        cob.transform.localScale = new Vector3Int(tileSize, tileSize, tileSize);
        cob.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Structure s = cob.GetComponent<Structure>();
        s.SetAttachedObject(cob);

        long ID = MakeStructureID();
        
        foreach(Tile a in affected){
            a.SetOccupied(s, ID);
        }
        s.OnPlace(placeable);
        return true;   
    }

    /// <summary><strong>Returns:</strong> the unique ID for this placed Structure.</summary>
    private long MakeStructureID()
    {
        long id = (long)UnityEngine.Random.Range(1, Int64.MaxValue);
        while (StructureIDs.Contains(id))
        {
            id = (long)UnityEngine.Random.Range(1, Int64.MaxValue);
        }
        StructureIDs.Add(id);
        return id;
    }

    ///<summary>  <strong>Returns:</strong> a possibly empty set of Tiles affected by placing <c>placeable</c> on Tile <c>t</c>.
    /// <br></br><em>Precondition</em>: both <c>t</c> and <c>placeable</c> are not <c>null</c>. </summary>
    private HashSet<Tile> AffectedTiles(IPlaceable placeable, Tile t)
    {
        Assert.IsNotNull(placeable, "Parameter placeable cannot be null.");
        Assert.IsNotNull(t, "Parameter t cannot be null.");

        HashSet<Tile> res = new HashSet<Tile>();

        Vector2Int tCoords = t.Coordinates();

        for (int x = 0; x < placeable.Bounds().x; x++)
        {
            for (int y = 0; y < placeable.Bounds().y; y++)
            {
                Tile w = TileByLoc(new Vector2Int(tCoords.x + x * tileSize, tCoords.y - y * tileSize));
                res.Add(w);
            }

        }
        return res;
    }

    /// <summary>If the user can place <c>placeable</c> on the current
    /// <c>tileHovering</c>, highlight the affected tiles blue.<br></br> If not, highlight them red.
    /// <br></br><em>Precondition: </em> <c>placeable</c> is not <c>null</c>.</summary>
    public void ShowTileAvailability(IPlaceable placeable)
    {
        Assert.IsNotNull(placeable, "Parameter placeable cannot be null.");
        if (tileHovering == null) return;

        HashSet<Tile> affected = AffectedTiles(placeable, tileHovering);
        UnHighlightTiles(affected);
        if (CanPlace(placeable, tileHovering)) HighlightTiles(affected, new Color32(52, 143, 235, 255)); // a nice green
        else HighlightTiles(affected, new Color32(245, 51, 51, 255)); // a nice red
    }

    /// <summary> Unhighlights all Tiles on this TileGrid.</summary>
    public void UnHighlightTiles()
    {
        foreach(Tile t in highligtedTiles)
        {
            t.SetColor(new Color32(255, 255, 255, 255));
        }
        highligtedTiles = new HashSet<Tile>();
    }

    /// <summary>Highlights all Tiles in <c>affected</c> with color <c>color</c>.</summary>
    private void HighlightTiles(HashSet<Tile> affected, Color32 color)
    {
        foreach(Tile t in affected)
        {
            if (t != null)
            {
                t.SetColor(color);
                highligtedTiles.Add(t);
            }
        }
    }

    //THIS IS FOR TESTING ONLY. LIGHTS UP ALL TILES IN TILES WITH RED.
    public static void LightUpTest(HashSet<Tile> tiles)
    {
        Debug.Log("Warning: You have called a method specifically implemented for testing.");
        foreach(Tile t in tiles)
        {
            t.SetColor(new Color32(245, 51, 51, 255));
        }
    }

    /// <summary>Empties Tile <c>t</c>.
    /// <br></br>If this Tile is part of a group of Tiles holding one Structure, empties all Tiles in that group too.
    /// <br></br><em>Precondition:</em> <c>t</c> is occupied.</summary>
    public void Empty(Tile t)
    {
        if (!t.IsOccupied()) return;
        long id = t.StructureID();
        t.Empty();
        foreach(Tile w in t.AllNeighbors())
        {
            if (w != null && w.IsOccupied() && id == w.StructureID()) 
            {
                Empty(w); 
            }
        }
    }

    /// <summary>Sets Tiles in <c>highlightedTiles</c> but not in <c>affected</c> back to their default color.</summary>
    private void UnHighlightTiles(HashSet<Tile> affected)
    {
        HashSet<Tile> deselected = new HashSet<Tile>();
        foreach(Tile t in highligtedTiles)
        {
            if (!affected.Contains(t)) t.SetColor(new Color32(255, 255, 255, 255));
            deselected.Add(t);
        }
        foreach(Tile t in deselected)
        {
            highligtedTiles.Remove(t);
        }
    }

    ///<summary> <strong>Returns:</strong> <c>true</c> if placing placeable on Tile <c>t</c> is allowed. </summary>
    private bool CanPlace(IPlaceable placeable, Tile t)
    {
        if (t == null || placeable == null) return false;

        HashSet<Tile> affected = AffectedTiles(placeable, t);
        foreach(Tile a in affected)
        {
            if (a == null || a.IsOccupied()) return false;
        }

        return true;
    }

    ///<summary> <strong>Returns:</strong> the Tile in this TileGrid at position <c>loc</c>, 
    ///or <c>null</c> if no such Tile exists. </summary>
    private Tile TileByLoc(Vector2Int loc)
    {
        foreach(Tile t in tiles)
        {
            if (t.Coordinates() == loc) return t;
        }
        return null;
    }

    /// <summary>Sets the Northern, Eastern, Southern, and Western neighbors for each Tile in this TileGrid.
    /// <br></br>If such a neighbor does not exist, sets the neighbor in that Direction to <c>null</c>.</summary>
    private void SetTileNeighbors()
    {
        foreach(Tile t in tiles)
        {
            Vector2Int tCoords = t.Coordinates();
            Tile northern = TileByLoc(new Vector2Int(tCoords.x, tCoords.y + tileSize));
            Tile eastern = TileByLoc(new Vector2Int(tCoords.x + tileSize, tCoords.y));
            Tile southern = TileByLoc(new Vector2Int(tCoords.x, tCoords.y - tileSize));
            Tile western = TileByLoc(new Vector2Int(tCoords.x - tileSize, tCoords.y));
            t.SetNeighbors(new Tile[] { northern, eastern, southern, western });
        }
    }

    ///<summary> <strong>Returns:</strong> the (height in Tiles) * (Tile size) of this TileGrid. </summary>
    public float Size()
    {
        return gridSize * tileSize;
    }

    ///<summary> <strong>Returns:</strong> the <c>transform.position</c> of a middle-most Tile in this TileGrid. There are four middle most 
    ///tiles in this TileGrid because its width and height are even.</summary>
    public Vector2 MiddleOfGrid()
    {
        Tile mid = TileByLoc(new Vector2Int(((tileSize * gridSize) / 2) - tileSize, ((tileSize * gridSize) / 2) - tileSize));
        return mid.transform.position;
    }

   

   
}
