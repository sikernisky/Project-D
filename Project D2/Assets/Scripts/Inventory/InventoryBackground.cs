using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// This class represents the Inventory's background.
/// </summary>
public class InventoryBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ///The Animator for the entire Inventory.
    [SerializeField]
    private Animator invBackAnim;

    /// <summary>True if the user released their mouse on the Inventory Background.</summary>
    private static bool hoveringInventory;

    private void Start()
    {
        invBackAnim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = false;
        hoveringInventory = true;
        //invBackAnim.SetTrigger("slideIn");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CameraControl.CanDragCamera = true;
        hoveringInventory = false;
        //invBackAnim.SetTrigger("slideOut");

    }

    /// <summary><strong>Returns:</strong> true if the user released their mouse on the inventory.</summary>
    public static bool OnInventory()
    {
        return hoveringInventory;
    }
}
