using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class StationInventory : Item, IConnectable, IPlaceable, IMoveable
{
    /// <summary> The width and height, in <c>Tiles</c>, of this <c>StationInventory</c>.</summary>
    private Vector2Int dimensions;

    /// <summary> The direction in which this StationInventory moves ItemBoxes. </summary>
    private Direction direction;

    /// <summary>Constructor: A Station with name <c>name</c> and dimensions <c>dimensions</c>.
    /// <br>Precondition:</br> <c>dimension</c> has x and y values > 0. </summary>
    public StationInventory(string name, Vector2Int dimensions) : base(name) {
        Assert.IsTrue(dimensions.x > 0 && dimensions.y > 0, "Parameter dimensions must contain values > 0.");
        this.dimensions = dimensions;
    }

    public Vector2Int Bounds()
    {
        return dimensions;
    }

    public abstract Vector2Int AttachRange();
    public abstract void OnPlace();
    public abstract GameObject PrefabObject();
    public abstract Sprite[] MoveAnimation();
    public abstract Direction Direction();
    public abstract void OnAccept();
    public abstract void OnClick();
}
