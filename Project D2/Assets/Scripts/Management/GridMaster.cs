using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates and manages all TileGrids in the scene.
/// </summary>
public class GridMaster : MonoBehaviour
{


    ///<summary>A list of ground Tile Sprites.</summary>
    [SerializeField]
    private List<Sprite> groundTileSprites;

    ///<summary>The Camera in the scene.</summary>
    [SerializeField]
    private CameraControl mainCamera;

    /// <summary>Size of this level's TileGrid. Set in inspector.</summary>
    [SerializeField]
    private int gridSize;

    /// <summary>Size of the spawned TileGrid's customer area. </summary>
    [SerializeField]
    private int customerAreaSize;

    /// <summary>The InventoryMaster in the scene. </summary>
    [SerializeField]
    private InventoryMaster inventoryMaster;

    /// <summary>The list of (x,y) positions to place a PowerCell when spawning a TileGrid.</summary>
    [SerializeField]
    private List<Vector2Int> powerCellPositions;


    private Dictionary<IPlaceable, List<Vector2Int>> ConstructPreSpawnDictionary()
    {
        Dictionary<IPlaceable, List<Vector2Int>> preSpawns = new Dictionary<IPlaceable, List<Vector2Int>>();
        preSpawns.Add(new PowerCell(), powerCellPositions);

        return preSpawns;
    }

    void Awake()
    {
        TileGrid grid = new TileGrid("TestGrid", 18, gameObject.transform, groundTileSprites, ConstructPreSpawnDictionary(), customerAreaSize);
        inventoryMaster.SetGrid(grid);
        mainCamera.SetGrid(grid);
    }


}
