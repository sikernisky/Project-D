using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// This class wraps some Item so that it can be moved across the scene.
/// </summary>
public class ItemBox : MonoBehaviour
{
    /// <summary>The Item in this ItemBox.</summary>
    private Item containedItem;

    /// <summary>true if this ItemBox contains some Item.</summary>
    private bool isBoxed;

    /// <summary>This ItemBox's SpriteRenderer component.</summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>Places <c>i</c> in this ItemBox, "wrapping" it.
    /// <br></br><em>Precondition:</em> <c>i</c> is not <c>null</c>.</summary>
    public void BoxItem(Item i)
    {
        Assert.IsNotNull(i, "Parameter i cannot be null.");
        if (spriteRenderer == null || GetComponent<SpriteRenderer>() == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = i.ItemBoxSprite();
        containedItem = i;
        isBoxed = true;
    }

    /// <summary><strong>Returns:</strong> the name of the Item this ItemBox is holding.</summary>
    public string BoxedItemName()
    {
        return containedItem.Name();
    }

    /// <summary><strong>Returns:</strong> true if this ItemBox contains some item.</summary>
    public bool IsEmpty()
    {
        return isBoxed;
    }
}
