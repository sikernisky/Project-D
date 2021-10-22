using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{

    public enum PathType { Sweet, Meat, Veggie}

    /**Path Type of this food.*/
    public abstract PathType PATH_TYPE { get; }

    /**Base reward of this food.*/
    public abstract int BASE_REWARD { get; }

    /**Name of this food.*/
    public abstract string NAME { get; }

    /**Returns true if this Food should perform its ability.*/
    public abstract bool AbilityConditionMet();

    /**Performs this Food's main ability.*/
    public abstract void PerformAbility(Plate plate);



}
