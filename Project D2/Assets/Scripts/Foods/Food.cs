using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : Item
{

    public enum PathType { Sweet, Meat, Veggie, Pathless}


    /**Path Type of this food.*/
    public virtual PathType PATH_TYPE { get; } = PathType.Pathless;

    /**Base reward of this food.*/
    public virtual int BASE_REWARD { get; } = 0;

    /**True if this food has been Roasted; false otherwise. */
    public virtual bool Roasted { get; set; }

    /**Name of this food.*/
    public override string NAME { get; } = "Food";

    /**Returns true if this Food should perform its ability.*/
    public abstract bool AbilityConditionMet();

    /**Performs this Food's main ability.*/
    public abstract void PerformAbility(Plate plate);
}
