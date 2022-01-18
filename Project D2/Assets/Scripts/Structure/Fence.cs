using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This class represents a Fence structure.
/// For more details on numberings ("fence4"), see the PNG guide.
/// </summary>
public class Fence : Structure
{

    [SerializeField]
    /// <summary>
    /// Each Sprite by Index:
    /// 0: Sprite representing this Fence connected by eastern and southern Fences.
    /// 1: Sprite representing this Fence connected by eastern and western Fences.
    /// 2: Sprite representing this Fence connected by southern and western Fences.
    /// 3: Sprite representing this Fence connected by no Fences.
    /// 4: Sprite representing this Fence connected by northern, eastern, southern, and western Fences.
    /// 5: Sprite representing this Fence connected by eastern, southern, and western Fences.
    /// 6: Sprite representing this Fence connected by northern and southern Fences.
    /// 7: Sprite representing this Fence connected by northern, eastern, and southern Fences.
    /// 8: Sprite representing this Fence connected by northern, southern, and western Fences.
    /// 9: Sprite representing this Fence connected by northern and eastern Fences.
    /// 10: Sprite representing this Fence connected by an eastern Fence.
    /// 11: Sprite representing this Fence connected by northern and western Fences.
    /// 12: Sprite representing this Fence connected by northern, eastern, and western Fences.
    /// 13: Sprite representing this Fence connected by a western Fence.
    /// 14: Sprite representing this Fence connected by a northern Fence.
    /// 15: Sprite representing this Fence connected by a southern Fence.
    /// </summary>
    private Sprite[] fenceSprites = new Sprite[14];


    public override void OnPlace(IPlaceable p)
    {
        base.OnPlace(p);
        SetFenceSprite(DetermineFenceSprite());
        foreach(Tile t in Surrounding())
        {
            Fence f = FenceOnTile(t);
            if (t != null && f != null) f.SetFenceSprite(f.DetermineFenceSprite());
        }
    }

    private Sprite DetermineFenceSprite()
    {
        List<int> candidates = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        Tile tileOn = null;
        foreach (Tile t in Occupying()) { tileOn = t; } //Guarenteed to be one because Fences occupy ONE Tile.

        Fence n = null;
        if (tileOn.NorthernNeighbor() != null) n = tileOn.NorthernNeighbor().Structure() as Fence;
        Fence e = null;
        if (tileOn.EasternNeighbor() != null) e = tileOn.EasternNeighbor().Structure() as Fence;
        Fence s = null;
        if (tileOn.SouthernNeighbor() != null) s = tileOn.SouthernNeighbor().Structure() as Fence;
        Fence w = null;
        if (tileOn.WesternNeighbor() != null) w = tileOn.WesternNeighbor().Structure() as Fence;

        if (n == null) RemoveSome(candidates, new int[] { 4, 6, 7, 8, 9, 11, 12, 14 });
        if (n != null) RemoveSome(candidates, new int[] { 0, 1, 2, 3, 5, 10, 13, 15 });

        if (e == null) RemoveSome(candidates, new int[] { 0, 1, 4, 5, 7, 9, 10, 12 });
        if (e != null) RemoveSome(candidates, new int[] { 2, 3, 6, 8, 11, 13, 14, 15 });

        if (s == null) RemoveSome(candidates, new int[] { 0, 2, 4, 5, 6, 7, 8, 15 });
        if (s != null) RemoveSome(candidates, new int[] { 1, 3, 9, 10, 11, 12, 13, 14 });

        if (w == null) RemoveSome(candidates, new int[] { 1, 2, 4, 5, 8, 11, 12, 13 });
        if (w != null) RemoveSome(candidates, new int[] { 0, 3, 6, 7, 9, 10, 14, 15 });

        foreach(int i in candidates) { Debug.Log(i); }

        Assert.IsTrue(candidates.Count == 1, "Candidate should have 1 Sprite remaining, but it has " + candidates.Count);
        return fenceSprites[candidates[0]];
    }

    /// <summary><strong>Returns:</strong> the Fence on <c>t</c> if there is one. Otherwise, returns <c>null</c>.</summary>
    private Fence FenceOnTile(Tile t)
    {
        if (t == null) return null;
        if (t.Structure() != null && t.Structure() is Fence) return t.Structure() as Fence;
        return null;
    }

    /// <summary>Removes all elements in <c>vals</c> from <c>candidates</c>.
    /// <br></br><em>Precondition:</em> <c>vals</c> and <c>candidates</c> are not <c>null</c>.</summary>
    private void RemoveSome(List<int> candidates, int[] vals)
    {
        Assert.IsNotNull(candidates, "Parameter vals cannot be null.");
        Assert.IsNotNull(candidates, "Parameter candidates cannot be null.");
        foreach(int i in vals)
        {
            candidates.Remove(i);
        }
    }

    /// <summary>Sets this Fence's Sprite. 
    /// <br></br>><em>Precondition:</em> <c>s</c> is not <c>null</c>.</summary>
    private void SetFenceSprite(Sprite s)
    {
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        Renderer().sprite = s;
    }

    private void Start()
    {
        foreach(Sprite s in fenceSprites) { Assert.IsNotNull(s, "You forgot to fill a Fence Sprite."); }
    }


}
