using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This class represents a Station. 
/// </summary>
public class Station : Mover
{

    /// <summary>A set of all Movers that this Station can move its ItemBox to. </summary>
    private HashSet<Mover> nextMovers = new HashSet<Mover>();

    /// <summary>Finds this Station's next Mover in the movement chain.
    /// <br></br>This is determined by a random Mover that this Station attaches to.</summary>
    protected override void FindNextMover()
    {
        return; // No logic is needed here, but we must override Mover.FindNextMover().
    }

    /// <summary>Finds the possibly empty set of Movers surrounding this Station, 
    /// and sets <c>nextMover</c> as the pointer to this set. </summary>
    private void FindSurroundingMovers()
    {
        HashSet<Tile> surrounding = Surrounding();
        HashSet<Mover> surroundingMovers = new HashSet<Mover>();
        foreach (Tile t in surrounding)
        {
            if (t.IsOccupied() && t.Structure() is Mover)
            {
                surroundingMovers.Add((Mover)t.Structure());
            }
        }
        nextMovers = surroundingMovers;
    }

    public override void OnPlace(IPlaceable p)
    {
        base.OnPlace(p);
        ShouldHoldItems(true);
    }


    /// <summary>(1) Attaches this Station's LineRenderer position 1 to the Mouse if it is not attached; if it is, releases the line.</summary>
    public override void OnClick(Tile t)
    {
        base.OnClick(t);
    }
}
