using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

/// <summary>
/// Represents some Object placed on a Tile.
/// </summary>
public class Structure : MonoBehaviour
{
    /// <summary>The Tiles this Structure is occupying by its placement. </summary>
    private HashSet<Tile> occupyingTiles = new HashSet<Tile>();

    /// <summary>The Tiles surrounding -- but not under -- this Structure. </summary>
    private HashSet<Tile> surroundingTiles = new HashSet<Tile>();

    /// <summary>The GameObject attached to this Structure.</summary>
    private GameObject ob;

    /// <summary>The Item this Structure is representing.</summary>
    private Item item;

    /// <summary>This Structure's SpriteRenderer component. </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>The (x,y) bounds of this Mover on its TileGrid. </summary>
    private Vector2Int bounds;

    /// <summary>The IPlaceable that placed/is this Structure.</summary>
    private IPlaceable placeable;

    /// <summary>This Structure's Connector component; <c>null</c> if it doesn't have one.</summary>
    [SerializeField]
    private Connector connector;

    /// <summary>True if a Connector can attach a line onto this Structure.</summary>
    [SerializeField]
    private bool canConnectTo;

    /// <summary>The list of Connectors attached TO this Structure. <c>connector</c> may not exist in this list.</summary>
    private List<Connector> attachedConnectors = new List<Connector>();

    /// <summary>true if a PowerSource is connecting to this Mover.</summary>
    private bool powered;


    /// <summary>Adds a Tile <c>t</c> to the HashSet of Tiles that this Structure is occupying.
    /// <br></br><em>Precondition:</em> <c>t</c> is not <c>null</c>. </summary>
    public void AddOccupyingTile(Tile t)
    {
        Assert.IsNotNull(t, "Parameter t cannot be null.");
        occupyingTiles.Add(t);
    }

    /// <summary>Removes a Tile <c>t</c> from the HashSet of Tiles that this Structure is occupying.
    /// <br></br><em>Precondition:</em> <c>t</c> is not <c>null</c>. 
    /// <br></br><em>Precondition:</em> <c>t</c> is in <c>occupyingTiles</c>.</summary>
    public void RemOccupyingTile(Tile t)
    {
        Assert.IsNotNull(t, "Parameter t cannot be null.");
        Assert.IsTrue(occupyingTiles.Contains(t), "Parameter t must exist in occupyingTiles.");
        occupyingTiles.Remove(t);
    }

    /// <summary><strong>Returns:</strong> the <c>transform.position</c> of this Structure.</summary>
    public Vector3 StructPos()
    {
        return AttachedObject().transform.position;
    }

    /// <summary><strong>Returns:</strong> The GameObject associated with this Structure.</summary>
    private GameObject AttachedObject()
    {
        return ob;
    }


    /// <summary>Removes this Structure's <c>ob</c> from the scene. </summary>
    public void DestroyAttachedObject()
    {
        if (ob != null) Destroy(ob);
    }

    /// <summary>Sets this Structure's attached object and spriteRenderer to its SpriteRenderer component.
    /// <br></br><em>Precondition:</em> <c>ob</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>ob</c> has a SpriteRenderer component. </summary>
    public virtual void SetAttachedObject(GameObject ob)
    {
        Assert.IsNotNull(ob, "Parameter ob cannot be null.");
        Assert.IsNotNull(ob.GetComponent<SpriteRenderer>(), "Parameter ob must have a SpriteRenderer component.");
        this.ob = ob;
        spriteRenderer = ob.GetComponent<SpriteRenderer>();
    }

    /// <summary>Adds connector c to this Structure's List of attached connectors.
    /// <br></br><em>Precondition:</em> <c>c</c> is not null. 
    /// <br></br><em>Precondition:</em> <c>c</c> does not exist in <c>attachedConnectors</c>.</summary>
    public void AddAttachedConnector(Connector c)
    {
        Assert.IsNotNull(c, "Paramter c cannot be null.");
        Assert.IsFalse(attachedConnectors.Contains(c), "Connector c cannot already exist in this Structure's attachedConnectors.");
        attachedConnectors.Add(c);
    }

    /// <summary>Removes connector c from this Structure's List of attached connectors.
    /// <br></br><em>Precondition:</em> <c>c</c> is not null. 
    /// <br></br><em>Precondition:</em> <c>c</c> exists in <c>attachedConnectors</c>.</summary>
    public void RemoveAttachedConnector(Connector c)
    {
        Assert.IsNotNull(c, "Paramter c cannot be null.");
        Assert.IsTrue(attachedConnectors.Contains(c), "Connector c must exist in this Structure's attachedConnectors.");
        attachedConnectors.Remove(c);
    }

    /// <summary>Removes the first connector (at index 0) from <c>attachedConnectors</c> and returns it.
    /// <br></br><em>Precondition:</em> <c>attachedConnectors</c> has Count > 0.</summary>
    private Connector PickupConnector()
    {
        Assert.IsTrue(attachedConnectors.Count > 0, "Cannot call PickupConnector() because attachedConnectors has length < 1.");
        Connector c = attachedConnectors[0];
        attachedConnectors.RemoveAt(0);
        return c;
    }

    /// <summary>Sets the Item this Structure is representing.
    /// <br></br><em>Precondition:</em> <c>i</c> is not <c>null</c>.</summary>
    public void SetItem(Item i)
    {
        Assert.IsNotNull(i, "Parameter i cannot be null.");
        item = i;
    }

    /// <summary><strong>Returns:</strong> true if this Structure is representing a Conveyor.</summary>
    public bool IsConveyor()
    {
        if (item == null) return false;
        return item is DefaultConveyor;
    }

    /// <summary><strong>Returns:</strong> true if this Structure is representing a Station.</summary>
    public bool IsStation()
    {
        if (item == null) return false;
        return item is StationInventory;
    }

    /// <summary><strong>Returns:</strong> a list of Connectors attached to this Structure. </summary>
    protected List<Connector> Connectors()
    {
        return attachedConnectors;
    }

    /// <summary><strong>Returns:</strong> the taken ItemBox. Takes, sets up, and places an ItemBox on this Structure.
    /// <br></br> This method is called ONCE per an ItemBox's lifetime.
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.</summary>
    public virtual ItemBox TakeItem(ItemBox ib)
    {
        GameObject ibob = ib.gameObject;
        ibob.transform.position = AttachedObject().transform.position;
        ibob.transform.SetParent(transform);
        ibob.transform.localScale = new Vector3(1, 1);

        SpriteRenderer ibRenderer = ibob.GetComponent<SpriteRenderer>();
        ibRenderer.sortingOrder = AttachedObject().GetComponent<SpriteRenderer>().sortingOrder + 1;

        return ib;
    }

    /// <summary><strong>Returns:</strong> a copy of the HashSet of Tiles that this Structure is occupying.</summary>
    protected HashSet<Tile> Occupying()
    {
        return new HashSet<Tile>(occupyingTiles);
    }

    /// <summary><strong>Returns:</strong> a copy of the HashSet of Tiles that surrounds this Structure's occupying Tiles.</summary>
    protected HashSet<Tile> Surrounding()
    {
        return new HashSet<Tile>(surroundingTiles);
    }

    protected HashSet<Tile> Surrounding(int thickness)
    {
        return FindSurroundingTiles(thickness);
    }

    /// <summary>Finds all Tiles surrounding this Structure's set of <c>occupyingTiles</c>.
    /// <br></br><em>Precondition:</em> field <c>occupyingTiles</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>thickness</c> > 0.</summary>
    protected HashSet<Tile> FindSurroundingTiles(int thickness = 1)
    {
        Assert.IsNotNull(Occupying(), "This Structure has no Occupying Tiles.");
        Assert.IsTrue(thickness > 0, "Parameter thickness must be > 0.");

        HashSet<Tile> occupying = Occupying();
        HashSet<Tile> surrounding = new HashSet<Tile>();

        for (int i = 0; i < thickness; i++)
        {
            foreach (Tile t in occupying)
            {
                foreach (Tile w in t.AllNeighbors())
                {
                    if (w!= null && (!w.IsOccupied() || !t.IsOccupied() || w.StructureID() != t.StructureID())) surrounding.Add(w);
                }
            }

            foreach (Tile t in surrounding)
            {
                occupying.Add(t);
            }
        }

        return surrounding;
    }

    /// <summary>Sets this Structure's set of surrounding Tiles.</summary>
    private void SetSurroundingTiles()
    {
        surroundingTiles = FindSurroundingTiles(1);
    }

    /// <summary> Does something when this Structure is placed on a TileGrid. 
    /// <br></br><em>Precondition:</em> <c>p</c> is not <c>null</c>.</summary>
    public virtual void OnPlace(IPlaceable p)
    {
        Assert.IsNotNull(p, "Parameter p cannot be null.");
        SetSurroundingTiles();
        bounds = p.Bounds();
        placeable = p;
    }

    /// <summary><strong>Returns:</strong> this Structure's SpriteRenderer component. </summary>
    public SpriteRenderer Renderer()
    {
        return spriteRenderer;
    }

    /// <summary><strong>Returns:</strong> true if this Structure has an active power source.</summary>
    protected bool HasPowerSource()
    {
        return powered;
    }

    /// <summary>Examines each Connector attached to this Structure. If any represents an active power source,
    /// <br></br> powers this Structure on. Otherwise, powers this Structure off. </summary>
    private void UpdatePower()
    {
        bool hasPower = false;
        foreach(Connector c in attachedConnectors)
        {
            PowerStructure ps = c.GetPossiblePowerSource();
            if (ps != null && ps.IsPowering(this)) hasPower = true;
        }
        if (hasPower) PowerOn();
        else PowerOff();
    }

    /// <summary>Does something when a Connector on <c>other</c> connects a line to this structure.</summary>
    public virtual void OnConnect(Connector other) 
    {
        UpdatePower();
    }

    /// <summary>Does something when a line, previously attached to this Structure from <c>other</c>, is picked u. </summary>
    public virtual void OnPickup(Connector other)
    {
        UpdatePower();
    }

    /// <summary>Does something when an active power source connects to this Structure.</summary>
    public virtual void PowerOn()
    {
        powered = true;
        Debug.Log("I have a power source!");
    }

    /// <summary>Does something when this Structure loses all power. a</summary>
    public virtual void PowerOff()
    {
        Debug.Log("I lost my power source!");
        powered = false;
    }


    /// <summary>Does something when this Structure is clicked. </summary>
    public virtual void OnClick(Tile t)
    {
        HandleConnnectorOnClick(t);
    }

    /// <summary>Handles the logic behind connector click events.</summary>
    private void HandleConnnectorOnClick(Tile t)
    {
        Connector c = Connector.ConnectorDragging;
        if(c != null)
        {
            if (c == connector) c.DropLine();
            else if (Connectable()) c.AttachLine(this);
            else if (!c.IsDraggingLine() && connector != null) connector.OnClick(t, this);
        }
        else
        {
            if (attachedConnectors.Count > 0) PickupConnector().PickupLine(this);
            else if (connector != null) connector.OnClick(t, this);
        }
    }

    /// <summary><strong>Returns:</strong> true if a Connector is allowed to attach onto this Structure.</summary>
    public bool Connectable()
    {
        return canConnectTo;
    }

    /// <summary><strong>Returns:</strong> the (x, y) bounds of this placed Structure. </summary>
    protected Vector2Int Bounds()
    {
        return bounds;
    }

    /// <summary><strong>Returns:</strong> this Structure's IPlaceable.</summary>
    public IPlaceable Placeable()
    {
        return placeable;
    }

    /// <summary><strong>Returns:</strong> the unique ID of this Structure.</summary>
    public long ID()
    {
        foreach(Tile t in Occupying())
        {
            return t.StructureID();
        }
        return default; // Never executed, Keeps C# Happy
    }







}
