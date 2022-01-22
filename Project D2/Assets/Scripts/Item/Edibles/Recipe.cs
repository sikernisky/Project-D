using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an Edible created by some combination of Edibles.
/// </summary>
public abstract class Recipe : Edible
{
    /// <summary>The list of Edibles that this Recipe comprises (possibly other Recipes). </summary>
    private List<Edible> ingredients;

    public Recipe(string name) : base(name) { }

}
