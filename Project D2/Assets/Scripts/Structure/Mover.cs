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
    private readonly float animDelay = .085f;

    /// <summary>The delay between each positional increment of a moved item. Smaller = faster. </summary>
    private readonly float moveSpeed = .001f;

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

    /// <summary>true if this Mover holds items instead of crashing them if there is no next target. </summary>
    [SerializeField]
    private bool holdItems;

    /// <summary>true if this Mover stops ItemBoxes at least once before sending them further. </summary>
    [SerializeField]
    private bool stopItems;

    /// <summary>true if this Mover finds its Next Mover when it is placed. </summary>
    [SerializeField]
    private bool autoSetNextMover;



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

    /// <summary>Moves a transform across this mover towards target and breaks it if <c>breakAfter</c> is true.</summary>
    IEnumerator MoveItemBoxAcross(ItemBox ib, Mover target)
    {
        AddBoxMoving(ib);
        Vector3 targetPos;
        if (target == null) targetPos = ContainerCrashLocation();
        else targetPos = target.StructPos();
        Transform ibPos = ib.transform;

        Vector3 totalDistance = targetPos - ibPos.position;
        Vector3 movementPerTick = totalDistance / 250;

        while(Vector3.Distance(ibPos.position + movementPerTick, targetPos) > .001)
        {
            ibPos.position += movementPerTick;
            yield return new WaitForSeconds(moveSpeed);
        }

        FindNextMover();
        if (NextMover() == null) Break(ib);
        else GiveToNextMover(ib, target);

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
        if (UsesPower()) Assert.IsTrue(HasPowerSource());
        Assert.IsTrue(itemBoxesQueued.Contains(ib));
        RemoveBoxQueued(ib);
        Mover next = NextMover();
        if (ib.IsMoving()) ib.StopMovementAnimation();
        ib.StartMovementAnimation(MoveItemBoxAcross(ib, next));
    }

    /// <summary>Gives this ItemBox to the next Mover. If <c>nextMover</c> is null, completes [LOGIC].
    /// <br></br><em>Precondition:</em> <c>ib</c> is not <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>ib</c> is in field <c>itemBoxesMoving</c>.</summary>

    private void GiveToNextMover(ItemBox ib, Mover m)
    {
        Assert.IsNotNull(ib, "Parameter ib cannot be null.");
        Assert.IsTrue(itemBoxesMoving.Contains(ib), "Parameter ib must exist in field itemBoxesMoving.");
        if (m != null)
        {
            RemoveBoxMoving(ib);
            m.AcceptMovedItem(ib);
        }
        else Break(ib);

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
        StartCoroutine(BreakDelay(ib));
    }

    /// <summary><strong>Returns:</strong> true if this Mover stops ItemBoxes before sending them further.</summary>
    protected bool StopsItems()
    {
        return stopItems;
    }

    /// <summary>Waits and then destroys <c>ib</c>.</summary>
    IEnumerator BreakDelay(ItemBox ib)
    {
        Debug.Log("In BreakDelay");
        ib.SetMovementAvailability(false);
        int ticks = 10;
        int scaleReduction = 3;
        float delay = .01f;
        Vector3 totalSizeDec = ib.transform.localScale / scaleReduction;
        Vector3 sizeDec = totalSizeDec / ticks;
        for(int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(delay);
            FindNextMover();
            if (NextMover() != null)
            {
                ib.transform.localScale = new Vector3(1, 1, 1);
                ib.SetMovementAvailability(true);
                GiveToNextMover(ib, NextMover());
                yield break;
            }
            ib.transform.localScale -= sizeDec;
        }
        ib.BreakItem();
        RemoveBoxMoving(ib);
        yield return new WaitForSeconds(.5f);
        Destroy(ib.gameObject);
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
        if(!ib.CanChangeMovement()) ib.SetMovementAvailability(true);
        AddBoxQueued(ib);
        if ((HasPowerSource() && UsesPower()) || !UsesPower()) MoveAcross(ib);
        return ib;
    }



    /// <summary><strong>Returns:</strong> the Tile this Mover is on. 
    /// <br></br><em>Precondition:</em> this Mover occupies only one Tile.</summary>
    private Tile TileOn()
    {
        Assert.IsTrue(Occupying().Count == 1, "You cannot call TileOn() because this Mover occupies more than one Tile.");
        foreach(Tile t in Occupying()) { return t; }
        return null;
    }

    /// <summary><strong>Returns:</strong> the location for an ItemBox to crash after leaving this Mover. 
    /// <br></br>If this Mover occupies one tile, the crash location is the next neighbor in its Direction. If it occupies more
    /// <br></br>than one tile, its crash location is itself. 
    /// </summary>
    private Vector3 ContainerCrashLocation()
    {
        if (Occupying().Count == 1)
        {
            Tile t = TileOn();
            if (Direction() == global::Direction.North) return t.NorthernNeighbor().transform.position;
            if (Direction() == global::Direction.East) return t.EasternNeighbor().transform.position;
            if (Direction() == global::Direction.South) return t.SouthernNeighbor().transform.position;
            if (Direction() == global::Direction.West) return t.WesternNeighbor().transform.position;
            return Vector3.zero; //Never executed, keeps C# happy
        }
        else return StructPos();
    }

    /// <summary><strong>Returns:</strong> the next Mover in this movement chain, or <c>null</c> if there isn't one.  
    /// <br></br>The next Mover is determined by the next Tile in this Mover's Direction.
    /// <br></br><em>Precondition:</em> This Structure occupies only one Tile.</summary>
    protected virtual void FindNextMover()
    {
        Assert.IsTrue(Occupying().Count == 1, "You cannot call FindNextMover() because this Mover occupies more than one Tile.");
        Tile occ = TileOn();
        SetNextMover(null);
        foreach(Tile t in Surrounding())
        {
            Structure s = t.Structure();
            Mover w = null;
            if(s != null) w = s as Mover;
            if (w != null && MakesMovementProgress(w)) 
            {
                SetNextMover(w);
                break;
            } 
        }
    }

    /// <summary><strong>Returns:</strong> true if <c>w</c> shares its Direction with this Mover and makes progres
    /// <br></br>towards that direction.
    /// <br></br><em>Precondition:</em> <c>w</c> is not <c>null</c>.</summary>
    private bool MakesMovementProgress(Mover w)
    {
        Assert.IsNotNull(w, "Parameter w cannot be null.");
        Direction d = Direction();
        Vector2Int mCoords = TileOn().Coordinates();
        Vector2Int wCoords = w.RandomOccupying().Coordinates(); // w might occupy multiple tiles, any random one works.

        if (d == global::Direction.North && wCoords.y > mCoords.y) return true;
        if (d == global::Direction.East && wCoords.x > mCoords.x) return true;
        if (d == global::Direction.South && wCoords.y < mCoords.y) return true;
        if (d == global::Direction.West && wCoords.x < mCoords.x) return true;

        return false;
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

    /// <summary><If <c>shouldHold</c> is true, sets this <c>holdItems</c> to true. If false, sets <c>holdItems</c> to false.
    /// <br></br><em>Precondition:</em> <c>holdItems</c> is false if <c>shouldHold</c> is true and vice versa.</summary>
    protected void ShouldHoldItems(bool shouldHold)
    {
        if (holdItems) Assert.IsFalse(shouldHold, "This mover is already set to hold items.");
        if (!holdItems) Assert.IsTrue(shouldHold, "This mover is already set to crash items.");
        holdItems = shouldHold;
    }


    private void Start()
    {
        if (moveOnStart) StartMovement();
    }


    /// <summary>Calls <c>base.OnClick()</c> and then rotates this mover 90 degrees to the left if possible.</summary>
    public override void OnClick(Tile t)
    {
        base.OnClick(t);
    }

    public override void OnTopKeyDown()
    {
        if (Rotatable()) RotateDirection(global::Direction.North);
    }

    /// <summary>Rotates this Mover 90 degrees to the right if possible.</summary>
    public override void OnRightKeyDown()
    {
        if (Rotatable()) RotateDirection(global::Direction.East);
    }
    public override void OnBotKeyDown()
    {
        if (Rotatable()) RotateDirection(global::Direction.South);
    }

    /// <summary>Rotates this Mover 90 degrees to the left if possible.</summary>
    public override void OnLeftKeyDown()
    {
        if (Rotatable()) RotateDirection(global::Direction.West);
    }

    /// <summary>Sets this Mover's Direction to <c>d</c>.</summary>
    private void SetDirection(Direction d)
    {
        direction = d;
        FindNextMover();
    }


    /// <summary>Rotates this Mover so that it faces Direction d and sets its movement direction to d.</summary>
    protected override void RotateDirection(Direction d)
    {   
        if(itemBoxesQueued.Count == 0 && itemBoxesMoving.Count == 0)
        {
            base.RotateDirection(d);
            SetDirection(d);
        }

    }

    /// <summary>Calls <c>Structure.OnPlace()</c> and finds this Mover's <c>nextMover</c>. </summary>
    public override void OnPlace(IPlaceable p)
    {
        base.OnPlace(p);
        if (p is IMoveable) controller = (IMoveable)p;
        SetNextMover(null);
        if (autoSetNextMover) FindNextMover();
        foreach (Tile t in Surrounding())
        {
            if (t.IsOccupied() && t.Structure() is Mover)
            {
                Mover m = (Mover)t.Structure();
                if (m.autoSetNextMover) m.FindNextMover();
            }
        }
    }

    public override void OnConnect(Connector other)
    {
        base.OnConnect(other);
        Mover m = other.GetComponent<Mover>();
        if (m != null) m.SetNextMover(this);
        UpdateMovementAnimation(HasPowerSource());
        if (HasPowerSource() || !UsesPower())
        {
            ReleaseQueue();
            if(m != null) m.ReleaseQueue();
        }
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
