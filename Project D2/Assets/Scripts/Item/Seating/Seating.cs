using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This class represents something that a Customer might sit in.</summary>
public abstract class Seating : Item, IPlaceable
{
    public Seating(string name) : base(name) { }

    public GameObject PrefabObject()
    {
        return Resources.Load<GameObject>("Prefabs/Seating/" + Name());
    }

    public abstract Vector2Int Bounds();
    public abstract void OnPlace();
}
