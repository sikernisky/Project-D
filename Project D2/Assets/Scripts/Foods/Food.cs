using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{

    public enum PathType { Sweet, Meat, Veggie}

    /**The FoodScriptable object of this Food. */
    public abstract FoodScriptable FoodScriptable { get; set; }

    /**Returns true if this Food should perform its ability.*/
    public abstract bool AbilityConditionMet();

    /**Performs this Food's main ability.*/
    public abstract void PerformAbility(Plate plate);



}
