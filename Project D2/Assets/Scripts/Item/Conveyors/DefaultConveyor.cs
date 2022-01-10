using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public enum Direction
{
    North,
    East,
    South,
    West
}

public class DefaultConveyor : Item, IPlaceable, IMoveable
{
    /// <summary> The direction in which this DefaultConveyor moves <c>ItemBoxes</c>. </summary>
    private Direction direction;

    /// <summary> The width and height, in <c>Tiles</c>, of this <c>DefaultConveyor</c>.</summary>
    private Vector2Int dimensions;

    /// <summary><strong>Constructor:</strong> A <c>DefaultConveyor</c> facing North with height and width 1.</summary>
    public DefaultConveyor() : base("DefaultConveyor") 
    {
        dimensions = new Vector2Int(1, 1);
    }

    public Vector2Int Bounds()
    {
        return dimensions;
    }

    public void OnPlace()
    {
        throw new System.NotImplementedException();
    }

    public GameObject PrefabObject()
    {
        GameObject ob = Resources.Load<GameObject>("Prefabs/Conveyors/Default");
        Assert.IsNotNull(ob.GetComponent<Mover>(), "The Prefab for " + Name() + " must contain a Structure component.");
        return ob;
    }

    public Sprite[] MoveAnimation()
    {
        return Resources.LoadAll<Sprite>("Conveyors/Default");
    }



    /// <summary>Sets the Direction of this DefaultConveyor.
    /// <br></br><em>Precondition: </em> <c>d</c> does not equal the current Direction.</summary>
    public void SetDirection(Direction d)
    {
        Assert.IsFalse(d == direction);
        direction = d;
    }

    public void MoveAcross(Item i)
    {
        throw new System.NotImplementedException();
    }

    public Direction Direction()
    {
        return direction;
    }
    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }

    public void OnAccept()
    {
        return;
    }

    public void OnClick()
    {
        return;
    }

    public int AttachRange()
    {
        return -1;
    }
}
