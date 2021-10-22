using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Conveyor : MonoBehaviour, IMover
{ 
    /**Name of this conveyor.*/
    public abstract string NAME { get; }


    /**Rewards the player the item's reward value. */
    public abstract void CashItemIn(GameObject item, int reward);

    /**Destroys an item it holds.
     * item must 
     */
    public abstract void DestroyItem(GameObject item);


    public abstract void GiveItem(GameObject item);


    public abstract void MoveItem(GameObject item);


    public abstract void TakeItem(GameObject item);

}
