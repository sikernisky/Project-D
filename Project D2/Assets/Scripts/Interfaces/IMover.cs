using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover
{
    void TakeItem(GameObject item);

    void MoveItem(GameObject item);

    void GiveItem(GameObject item);

    void DestroyItem(GameObject item);

    void CashItemIn(GameObject item, int reward);
}
