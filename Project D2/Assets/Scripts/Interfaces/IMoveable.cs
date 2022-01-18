using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> This interface represents an <c>IPlaceable</c> that also moves <c>Items</c>.</summary>
public interface IMoveable
{
    /// <summary><strong>Returns:</strong> an array representing this <c>IMoveable</c>'s movement animation track.</summary>
    public Sprite[] MoveAnimation();

    /// <summary><strong>Returns:</strong> the Direction this <c>IMoveable</c> moves its <c>Item</c> towards.</summary>
    public Direction Direction();

    /// <summary>Does something when this IMoveable is accepted by a new Mover.</summary>
    public void OnAccept();

    /// <summary>Does something when this IMoveable is clicked. </summary>
    public void OnClick();

    /// <summary><strong>Returns:</strong> the name of this IMoveable.</summary>
    public string Name();

    /// <summary><strong>Returns:</strong> The range of this IMoveable's LineRenderer, or -1 if it does not have one.</summary>
}
