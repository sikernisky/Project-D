using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Conveyor : Item, IMover
{
    /**Name of this conveyor.*/
    public override string NAME { get; } = "Conveyor";

    public abstract void DestroyItem(GameObject item);


    public abstract void GiveItem(GameObject item);


    public abstract void MoveItem(GameObject item);


    public abstract void TakeItem(GameObject item);

}
