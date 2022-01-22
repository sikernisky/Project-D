using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory Item", menuName ="Scriptables/Inventory")]
public class InventoryItemScriptable : ScriptableObject
{
    /// <summary>The Sprite representing this <c>Item</c> in the inventory.</summary>
    [SerializeField]
    private Sprite slotSprite;

    /// <summary>The unique slot background Sprite for this <c>Item</c>.</summary>
    [SerializeField]
    private Sprite slotBackground;

    /// <summary>The name of the <c>Item</c> corresponding to this <c>InventoryItemScriptable</c>.</summary>
    [SerializeField]
    private string itemName;

    public List<int> a;

    public Item item;

}
