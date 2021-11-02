using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover
{
    void TakeMovedItem(GameObject item);

    void MoveItem(GameObject item);

    void GiveMovedItem(GameObject item, FluidItem target);

    void DestroyMovedItem(GameObject item);

    void CashMovedItemIn(GameObject item);
}
