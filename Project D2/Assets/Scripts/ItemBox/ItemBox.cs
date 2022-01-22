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

    /// <summary>true if this ItemBox crashed/broke in the scene.</summary>
    private bool broken;
    
    /// <summary>true if this ItemBox can be redirected to a different destination. </summary>
    private bool canChangeMovement;

    /// <summary>true if this ItemBox should break when it lands on the next mover. </summary>
    private bool breakNextLanding;

    /// <summary>true if this ItemBox is moving. </summary>
    private bool moving;

    /// <summary>The IEnumerator representing this ItemBox moving across some Mover, <c>null</c> if it isn't moving. </summary>
    private IEnumerator movementAnimation;

    /// <summary>Places <c>i</c> in this ItemBox, "wrapping" it.
    /// <br></br><em>Precondition:</em> <c>i</c> is not <c>null</c>.</summary>
    public void BoxItem(Item i)
    {
        Assert.IsNotNull(i, "Parameter i cannot be null.");
        if (spriteRenderer == null || GetComponent<SpriteRenderer>() == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        SetSprite(i.ItemBoxSprite());
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

    /// <summary><strong>Returns: </strong>the Item this ItemBox is wrapping/containing/holding.</summary>
    private Item ItemHolding()
    {
        return containedItem;
    }

    /// <summary>Sets this ItemBox's Sprite. 
    /// <br></br><em>Precondition:</em> <c>s</c> is not <c>null</c>.</summary>
    private void SetSprite(Sprite s)
    {
        Assert.IsNotNull(s, "Parameter s cannot be null.");
        spriteRenderer.sprite = s;
    }

    /// <summary>Breaks this ItemBox. 
    /// <br></br><em>Precondition:</em> the Item this ItemBox is holding is not <c>null</c>.
    /// <br></br><em>Precondition:</em> this ItemBox is not already broken.</summary>
    public void BreakItem()
    {
        Item i = ItemHolding();
        Assert.IsNotNull(i, "You cannot break this ItemBox because its wrapped item is null.");
        Assert.IsFalse(broken, "This ItemBox is already broken.");
        SetSprite(i.BreakSprite());
        broken = true;
    }

    /// <summary><strong>Returns:</strong> true if this ItemBox has crashed/broke.</summary>
    public bool IsBroken()
    {
        return broken;
    }


    /// <summary>Starts and sets this ItemBox's movement animation to <c>anim</c>.
    /// <br></br><em>Precondition:</em> <c>anim</c> is not <c>null</c> and then <c>movementAnimation</c> is.</summary>
    public void StartMovementAnimation(IEnumerator anim)
    {
        Assert.IsNull(movementAnimation, "This ItemBox's movement animation is already set.");
        movementAnimation = anim;
        StartCoroutine(movementAnimation);
        moving = true;
    }

    /// <summary>Stops this ItemBox's movement animation and sets <c>movementAnimation</c> to <c>null</c>.
    /// <br></br><em>Precondition:</em> <c>anim</c> is not <c>null</c>.</summary>
    public void StopMovementAnimation()
    {
        Assert.IsNotNull(movementAnimation, "You cannot stop this movement because it isn't moving.");
        StopCoroutine(movementAnimation);
        movementAnimation = null;
        moving = false;
    }

    /// <summary>Sets whether this ItemBox's path/direction can be changed.
    /// <br></br><em>Precondition:</em> <c>canChange</c> is false if <c>canChangeMovement</c> is true. 
    /// <br></br><em>Precondition:</em> <c>canChange</c> is true if <c>canChangeMovement</c> is false.</summary>
    public void SetMovementAvailability(bool canChange)
    {
        if (canChangeMovement) Assert.IsFalse(canChange, "This ItemBox can already change directions.");
        if (!canChangeMovement) Assert.IsTrue(canChange, "This ItemBox is already set to not change directions."); 
        canChangeMovement = canChange;
    }

    /// <summary><strong>Returns:</strong> true if this ItemBox is moving. </summary>
    public bool IsMoving()
    {
        return moving;
    }

    /// <summary><strong>Returns:</strong> true if this ItemBox can change its current direction/pathing.</summary>
    public bool CanChangeMovement()
    {
        return canChangeMovement;
    }
}
