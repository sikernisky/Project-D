using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents something that can be placed on a Tile in a TileGrid.
/// </summary>
public interface IPlaceable
{
    /// <summary><strong>Returns:</strong> the width and height, in Tiles, of this Placeable.</summary>
    Vector2Int Bounds();

    /// <summary> Do something when this Placeable is placed.</summary>
    void OnPlace();

    ///<summary><strong>Returns:</strong> The Object representing this Placeable.
    ///<br></br><em>Precondition:</em> The returned GameObject contains a Structure component.</summary>
    GameObject PrefabObject();
}
