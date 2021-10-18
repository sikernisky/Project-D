using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInventoryItem
{

    bool InInventory { get; set; }

    void DragItem();
    void OnPlace();
}
