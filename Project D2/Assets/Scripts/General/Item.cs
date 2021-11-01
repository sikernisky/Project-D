using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IMover
{
    public abstract string NAME { get; }

    public GameTile GameTileIn { get; protected set; }

    public void AssignGameTile(GameTile gameTile)
    {
        if (gameTile != null) GameTileIn = gameTile;
    }

    public abstract void CashItemIn(GameObject item);

    public abstract void DestroyItem(GameObject item);


    /**Gives an item it holds to another IMover.*/
    public virtual void GiveItem(GameObject item, Item target)
    {
        if (target != null && GameTileIn.NextGameTile.TileSetUp)
        {
            Debug.Log("Target is at position " + GameTileIn.NextGameTile.X + ", " + GameTileIn.NextGameTile.Y);
            target.TakeItem(item);
        }
        else DestroyItem(item);
    }

    public abstract void MoveItem(GameObject item);

    public abstract void TakeItem(GameObject item);
}
