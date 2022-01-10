using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This class represents some Structure occupying ONE tile that can move an ItemBox.
/// </summary>
public class Mover : Structure
{
    /// <summary>The delay between each tick of this Mover's animations. </summary>
    private readonly float animDelay = .075f;

    /// <summary>The number of position increments an item goes through while moving across this Mover. </summary>
    [SerializeField]
    private int moveTicks;

    /// <summary>true if this Mover needs to be connected to a PowerStructure to move ItemBoxes.</summary>
    [SerializeField]
    private bool needsPower;

    /// <summary>The <c>IMoveable</c> this Mover is imitating.</summary>
    private IMoveable controller;

    /// <summary>This Mover's animation track. </summary>
    [SerializeField]
    private Sprite[] movementAnimTrack;

    /// <summary>The Coroutine started when animating this Mover's movement. </summary>
    private IEnumerator movementCoroutine;

    /// <summary> Starts moving this Mover on Start() if true. </summary>
    [SerializeField]
    private bool moveOnStart;

    ///<summary>The Direction of this Mover.</summary>
    private Direction direction;

    /// <summary>All ItemBoxes this Mover is moving but not holding.</summary>
    private List<ItemBox> itemBoxesMoving = new List<ItemBox>();

    /// <summary>All ItemBoxes this Mover is holding but not moving. </summary>
    private List<ItemBox> itemBoxesQueued = new List<ItemBox>();

    /// <summary> true if this Mover is currently moving.</summary>
    private bool isMoving;

    /// <summary>The Mover that comes after this Mover; <c>null</c> if this Mover is the last.</summary>
    private Mover nextMover;



    /// <summary>Plays this Mover's movement animation if its track is not empty or <c>null</c>.</summary>
    IEnumerator PlayMovementAnimation()
    {
        int counter = 0;
        while (movementAnimTrack != null && movementAnimTrack.Length > 0)
        {
            Renderer().sprite = movementAnimTrack[counter];
            yield return new WaitForSeconds(animDelay);
            if (counter + 1 == movementAnimTrack.Length) counter = 0;
            else counter++;
        }
    }

    /// <summary>Moves a transform across this mover towards target.</summary>
    IEnumerator MoveItemBoxAcross(ItemBox ib)
    {
        Mover target = NextMover();
        Vector3 targetPos = target.StructPos();
        Transform ibTransform = ib.transform;

        Vector3 movementPerTick = (targetPos - ibTransform.position) / moveTicks;

        for(int i = 0; i < moveTicks; i++)
        {
            ibTransform.position += movementPerTick;
            yield return new WaitForSeconds(animDelay);
        }
        GiveToNextMover(ib);

    }

    /// <summary>Stops this Mover's movement animation.
    /// <br></br><em>Precondition:</em> the movement animation is already playing.</summary>
    public void StopMovement()
    {
        Assert.IsNotNull(movementCoroutine, "The movement animation must be playing.");
        Assert.IsTrue(isMoving, "Field isMoving is false, indicating that this Mover is not moving.");
        isMoving = false;
        StopCoroutine(movementCoroutine);
        movementCoroutine = null;
    }

    /// <summary>Starts this Mover's movement animation.
    /// <br></br><em>Precondition:</em> the movement animation is not playing.</summary>
    public void StartMovement()
    {
        Assert.IsNull(movementCoroutine, "The movement animation cannot already be playing.");
        Assert.IsFalse(isMoving, "Field isMoving is true, indicating that this Mover is already moving.");
        isMoving = true;
        movementCoroutine = PlayMovementAnimation();
        StartCoroutine(movementCoroutine);
    }

    /// <summary>Moves ItemBox <c>ib</c> across this Mover before sending it to <c>nextMover.</c>
    /// <br></br>Removes <c>ib</c> from the ItemBox queue and adds it to the list of moving ItemBoxes.
    /// <br></br>If this Mover's <c>nextMover</c> is null, cashes or crashes <c>ib</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> this Mover has an active power source if it requires one.
    /// <br></br><em>Precondition:</em> <c>ib</c> exists in <c>itemBoxesQueued</c>.</summary>
    public void MoveAcross(ItemBox ib)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        if (needsPower) Assert.IsTrue(HasPowerSource());
        Assert.IsTrue(itemBoxesQueued.Contains(ib));

        Mover next = NextMover();
        RemoveBoxQueued(ib);
        AddBoxMoving(ib);
        if (next == null)
        {
            Break(ib); // Destroy the ItemBox if there's no Mover to accept it.
            return;
        }
        StartCoroutine(MoveItemBoxAcross(ib));
    }

    /// <summary>Gives this ItemBox to the next Mover. If <c>nextMover</c> is null, completes [LOGIC].
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is in field <c>itemBoxesMoving</c>.</summary>

    private void GiveToNextMover(ItemBox ib)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        Assert.IsTrue(itemBoxesMoving.Contains(ib), "Parameter ib must exist in field itemBoxesMoving.");
        RemoveBoxMoving(ib);
        NextMover().AcceptMovedItem(ib);
    }

    /// <summary><strong>Returns:</strong> true if this Mover is moving.</summary>
    private bool IsMoving()
    {
        return isMoving;
    }

    /// <summary><strong>Returns:</strong> the Direction of this Mover.</summary>
    public Direction Direction()
    {
        return direction;
    }

    /// <summary> Destroys the ItemBox this Mover is holding.</summary>
    private void Break(ItemBox ib)
    {
        Debug.Log("Breaking ib");
        Destroy(ib.gameObject);
        RemoveBoxMoving(ib);
    }

    /// <summary>Sets this Mover's controller.
    /// <br></br><em>Precondition:</em> <c>controller</c> is not <c>null</c>.</summary>
    public void SetController(IMoveable controller)
    {
        Assert.IsNotNull(controller, "Parameter controller cannot be null.");
        this.controller = controller;
        this.controller.OnAccept();
    }

    /// <summary>Sets this Mover's <c>nextMover</c>. Sets it to <c>null</c> if there isn't one.</summary>
    protected virtual void SetNextMover(Mover m)
    {
        nextMover = m;
    }

    /// <summary> Accepts an ItemBox from another Mover. Completes necessary logic. </summary>
    private void AcceptMovedItem(ItemBox ib)
    {
        controller.OnAccept();
        TakeItem(ib);
    }

    /// <summary>Places <c>ib</c>'s GameObject ontop of this Mover and adds it to the ItemBox queue.
    /// <br></br>If this Mover needs and has an active power source, or if it doesn't need one at all,
    /// moves <c>ib</c> across this Mover immediately.
    /// <br></br><em>Precondition:</em> ib is not <c>null</c>.</summary>
    public override ItemBox TakeItem(ItemBox ib)
    {
        ItemBox takenBox = base.TakeItem(ib);
        AddBoxQueued(ib);
        if ((HasPowerSource() && needsPower) || !needsPower) MoveAcross(ib);
        return ib;
    }

    /// <summary>Calls <c>Structure.OnPlace()</c> and finds this Mover's <c>nextMover</c>. </summary>
    public override void OnPlace(IPlaceable p)
    {
        base.OnPlace(p);
        if (p is IMoveable) controller = (IMoveable)p;
        FindNextMover();
        foreach(Tile t in Surrounding())
        {
            if(t.IsOccupied() && t.Structure() is Mover)
            {
                Mover m = (Mover)t.Structure();
                m.FindNextMover();
            }
        }
    }


    /// <summary><strong>Returns:</strong> true if Mover <c>m</c> is in Range of this Mover's LineRenderer.</summary>
    protected bool InRange(Mover m)
    {
        if (controller.AttachRange() < 1) return false;
        HashSet<Tile> surr = FindSurroundingTiles(controller.AttachRange());
        HashSet<Tile> mOcc = m.Occupying();
        foreach(Tile t in mOcc)
        {
            if (surr.Contains(t)) return true;
        }
        return false;
    }

    /// <summary><strong>Returns:</strong> the Tile this Mover is on. 
    /// <br></br><em>Precondition:</em> this Mover occupies only one Tile.</summary>
    private Tile TileOn()
    {
        Assert.IsTrue(Occupying().Count == 1, "You cannot call TileOn() because this Mover occupies more than one Tile.");
        foreach(Tile t in Occupying()) { return t; }
        return null;
    }

    /// <summary><strong>Returns:</strong> the next Mover in this movement chain, or <c>null</c> if there isn't one.  
    /// <br></br>The next Mover is determined by the next Tile in this Mover's Direction.
    /// <br></br><em>Precondition:</em> This Structure occupies only one Tile.</summary>
    protected virtual void FindNextMover()
    {
        Assert.IsTrue(Occupying().Count == 1, "You cannot call FindNextMover() because this Mover occupies more than one Tile.");
        Tile occ = null;
        occ = TileOn(); // Guaranteed to be the only one.
        Direction d = Direction();
        if (d == global::Direction.North)
        {
            if (occ.NorthernNeighbor().IsOccupied() && occ.NorthernNeighbor().Structure() is Mover)
            {
                SetNextMover((Mover)occ.NorthernNeighbor().Structure());
            }
        }
        else if (d == global::Direction.East)
        {
            if (occ.EasternNeighbor().IsOccupied() && occ.EasternNeighbor().Structure() is Mover)
            {
                SetNextMover((Mover)occ.EasternNeighbor().Structure());
            }
        }
        else if (d == global::Direction.South)
        {
            if (occ.SouthernNeighbor().IsOccupied() && occ.SouthernNeighbor().Structure() is Mover)
            {
                SetNextMover((Mover)occ.SouthernNeighbor().Structure());
            }
        }
        else if (d == global::Direction.West)
        {
            if (occ.WesternNeighbor().IsOccupied() && occ.WesternNeighbor().Structure() is Mover)
            {
                SetNextMover((Mover)occ.WesternNeighbor().Structure());
            }
        }
        else SetNextMover(default);
    }

    /// <summary><strong>Returns:</strong> The Mover that will take/recieve an ItemBox moved by this one.</summary>
    protected Mover NextMover()
    {
        return nextMover;
    }

    /// <summary>Adds an ItemBox to <c>itemBoxesMoving</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.</summary>
    protected void AddBoxMoving(ItemBox ib)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        itemBoxesMoving.Add(ib);
    }

    /// <summary>Adds an ItemBox to <c>itemBoxesQueued</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.</summary>
    protected void AddBoxQueued(ItemBox ib)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        itemBoxesQueued.Add(ib);
    }

    /// <summary>Removes an ItemBox from <c>itemBoxesQueued</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is in <c>itemBoxesQueued</c>.</summary>
    protected void RemoveBoxQueued(ItemBox ib)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        Assert.IsTrue(itemBoxesQueued.Contains(ib), "The ItemBox you are trying to remove is not being carried by this Mover.");
        itemBoxesQueued.Remove(ib);
    }

    /// <summary>Removes an ItemBox from <c>itemBoxesMoving</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is in <c>itemBoxesMoving</c>.</summary>
    protected void RemoveBoxMoving(ItemBox ib)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        Assert.IsTrue(itemBoxesMoving.Contains(ib), "The ItemBox you are trying to remove is not being carried by this Mover.");
        itemBoxesMoving.Remove(ib);
    }

    /// <summary>Lights up this Mover, indicating that it is attached to some other Mover. 
    /// <br></br><em>Precondition: This Mover's SpriteRenderer is  not <c>null</c>.</summary>
    internal void SetMoverColor(Color32 c)
    {
        Assert.IsNotNull(Renderer(), "Parameter m.AttachedObject().Renderer() cannot be null.");
        Renderer().color = c;
    }



    private void Start()
    {
        if (moveOnStart) StartMovement();
    }

    public override void OnClick(Tile t)
    {
        base.OnClick(t);
    }

    public override void OnConnect(Connector other)
    {
        base.OnConnect(other);
        Mover m = other.GetComponent<Mover>();
        if (m != null) m.SetNextMover(this);
        UpdateMovementAnimation(HasPowerSource());
        if (HasPowerSource()) ReleaseQueue();
    }

    public override void OnPickup(Connector other)
    {
        base.OnPickup(other);
        FindNextMover();
        UpdateMovementAnimation(HasPowerSource());
    }

    /// <summary>Moves all ItemBoxes in this Mover's <c>itemBoxQueue</c> across this Mover. 
    /// <br></br>Calling MoveAcross() on an ItemBox implicitly removes it from <c>itemBoxesQueued</c>.</summary>
    private void ReleaseQueue()
    {
        for(int i = 0; i < itemBoxesQueued.Count; i++)
        {
            MoveAcross(itemBoxesQueued[i]);
        }
    }

    /// <summary>Starts this Mover's movement animation if <c>start</c> is true. If <c>start</c> is false,
    /// <br></br>stops it instead. </summary>
    private void UpdateMovementAnimation(bool start)
    {
        if (start)
        {
            if (movementCoroutine == null && !isMoving) StartMovement();
        }
        else
        {
            if (movementCoroutine != null && isMoving) StopMovement();
        }
    }

    public override void PowerOn()
    {
        base.PowerOn();
    }

}
