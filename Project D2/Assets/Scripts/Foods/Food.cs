using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{

    public enum PathType { Sweet, Meat, Veggie}
    public abstract int BaseReward { get; set; }
    public abstract string Name { get; set; }
    public abstract PathType Path { get; set; }

    public bool AbilityConditionMet() { return false; }
    public abstract void PerformAbility();


}
