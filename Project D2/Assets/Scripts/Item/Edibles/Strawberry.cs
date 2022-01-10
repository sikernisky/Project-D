using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : Edible
{
    /// <summary><strong>Constructor:</strong> A <c>Strawberry</c> with name "Strawberry" and Trait Pseduocarp.</summary>
    public Strawberry() : base("Strawberry") {
        AddTrait(Trait.Pseudocarp);
        SetPath(PathType.Sweets);
    }

    /// <summary>Attempts to perform the <c>Strawberry</c> ability.</summary>
    public override void TryAbility()
    {
        throw new System.NotImplementedException();
    }

    public override ItemBox BoxItem()
    {
        throw new System.NotImplementedException();
    }
}
