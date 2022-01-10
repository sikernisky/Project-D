using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

/// <summary>
/// This class represents a slot in the Inventory.
/// </summary>
public class InventorySlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
    /// <summary> The Item in this slot.</summary>
    private Item slotItem;

    /// <summary>The <c>Image</c> component for this <c>InventorySlot</c>'s background.</summary>
    [SerializeField]
    private Image background;

    /// <summary>The Canvas in the scene.</summary>
    [SerializeField]
    private Canvas canvas;

    /// <summary>The draggable <c>Image</c> component for this <c>InventorySlot</c>.</summary>
    [SerializeField]
    private Image draggable;

    /// <summary>The GameObject containing the <c>draggable</c> Image component.</summary>
    [SerializeField]
    private GameObject itemObject;

    /// <summary>The position of the draggable item when it sits in its slot.</summary>
    private Vector3 slotPosition;

    /// <summary>The InventoryMaster controlling this InventorySlot. </summary>
    private InventoryMaster master;

    /// <summary>The CanvasGroup on this InventorySlot. </summary>
    private CanvasGroup canvasGroup;

    private void Start()
    {
        slotPosition = itemObject.transform.position;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>Assigns a new InventoryMaster to this InventorySlot.
    /// <br></br><em>Precondition:</em> <c>master</c> is not <c>null</c>.</summary>
    public void NewMaster(InventoryMaster master)
    {
        Assert.IsNotNull(master, "Parameter master cannot be null.");
        this.master = master;
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        CameraControl.CanDragCamera = false;
        itemObject.transform.position = eventData.position;
        if(!InventoryBackground.OnInventory()) slotItem.OnDragInventory(master.MainGrid());
    }

    /// <summary>Fills this <c>InventorySlot</c> with <c>item</c>.</summary>
    /// <br></br><em>Precondition:</em> <c>item</c> is not <c>null</c>.
    public void FillSlot(Item item)
    {
        Assert.IsNotNull(item, "Parameter item cannot be null.");

        slotItem = item;
        background.sprite = item.InventorySlot();
        draggable.sprite = item.InventorySprite();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        CameraControl.CanDragCamera = true;
        itemObject.transform.position = slotPosition;
        master.MainGrid().UnHighlightTiles();
        if (!InventoryBackground.OnInventory())
        {
            slotItem.OnDropInventory(master.MainGrid());
        }
    }
}
