using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TabButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>The <c>InventoryTab</c> this <c>TabButton</c> controls. </summary>
    [SerializeField]
    private InventoryTab tab;

    /// <summary>The <c>Button</c> component of this <c>TabButton</c>. </summary>
    [SerializeField]
    private Button button;

    /// <summary>The <c>Image</c> component of this <c>TabButton</c>.</summary>
    [SerializeField]
    private Image tabImage;

    /// <summary>The text on this <c>TabButton</c>.</summary>
    [SerializeField]
    private TMP_Text buttonText;

    ///<summary>The <c>InventoryMaster</c> to implement this Tab's content into the inventory.</summary>
    [SerializeField]
    private InventoryMaster master;

    /// <summary><c>true</c> if this <c>TabButton</c> is actively controlling an <c>InventoryTab</c>.</summary>
    private bool active;

    public void OnTabClick()
    {
        master.RefreshInventory(tab);
    }

    /// <summary> Turns on this <c>TabButton</c> and assigns its <c>InventoryTab</c>.
    /// <br></br><em>Precondition:</em> tab is not <c>null</c> and its list of <c>Items</c> is not empty.
    /// <br></br><em>Precondition:</em> <c>active</c> is false.</summary>
    public void Activate(InventoryTab tab)
    {
        Assert.IsNotNull(tab, "Parameter tab cannot be null.");
        Assert.IsTrue(tab.Size() > 0, "Parameter tab must have size > 0.");
        Assert.IsFalse(active, "Parameter active must be false.");

        this.tab = tab;
        gameObject.SetActive(true);
        buttonText.text = tab.Name();
        active = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = true;
    }
}
