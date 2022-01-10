using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> The path this <c>Edible </c>is in. Every <c>Edible</c> contains at least one <c>PathType</c>. </summary>
public enum PathType
{
    Sweets,
    Veggies,
    Meats
}

/// <summary> Traits that an <c>Edible</c> may posess. </summary>
public enum Trait
{
    Pseudocarp
}

/// <summary> This class represents a food-like <c>Item</c>.</summary>
public abstract class Edible : Item
{
    /// <summary>This <c>Edible's</c> traits.</summary>
    private HashSet<Trait> traits = new HashSet<Trait>();

    /// <summary>This <c>Edible's</c> path.</summary>
    private PathType path;

    /// <summary><strong>Constructor: </strong>An Edible with name <c>name</c>.</summary>
    public Edible(string name): base(name) { }

    /// <summary>Adds a <c>Trait</c> to this <c>Edible</c>.</summary>
    protected void AddTrait(Trait t)
    {
        traits.Add(t);
    }

    /// <summary>Sets the <c>PathType</c> of this <c>Edible</c> </summary>
    protected void SetPath(PathType p)
    {
        path = p;
    }

    /// <summary>Attempts to perform this <c>Edible</c>'s main ability. <br></br>
    /// This method should be continuously called in Update by a MonoBehaviour when in play.</summary>
    public abstract void TryAbility();
}
