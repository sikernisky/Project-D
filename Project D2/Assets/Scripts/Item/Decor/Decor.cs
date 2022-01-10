using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This class represents an Item that acts as decoration (even if it does something). </summary>
public abstract class Decor : Item
{
    protected Decor(string name) : base(name) { }

    public GameObject PrefabObject()
    {
        return Resources.Load<GameObject>("Prefabs/Decors/" + Name());
    }
}
