using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHolder : MonoBehaviour
{
    public bool PlayerCanDragInto;

    public int HolderNumber;

    public Station ParentStation;

    public Coroutine HolderAnimation;

    public SpriteRenderer HolderSpriteRenderer;

    public GameObject HolderGameObject;

    public bool HoldingObject { get; private set; }

    public Vector2 HolderPosition { get; private set; }

    public GameObject ItemHolding { get; private set; }

    public StationHolder HoldItem(GameObject itemToHold)
    {
        if (!HoldingObject)
        {
            ItemHolding = itemToHold;
            HoldingObject = true;
            itemToHold.transform.localPosition = transform.localPosition;
            return this;
        }
        return null;
    }

    public void ReleaseItem()
    {
        ItemHolding = null;
        HoldingObject = false;
    }

    private void Start()
    {
        HolderPosition = transform.localPosition;
    }




}
