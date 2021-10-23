using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder
{
    private const float cellSize = 2f;
    
    /**Depending on levelNumber, set up THREE grids:
     * 1. The grid for base/ground tiles. 
     * 2. The grid for stations and conveyors.
     * 3. The grid for placeables, like plates.
     * The levelNumber determines customization.
     */
    public static void SetUpGrid(int levelNumber)
    {
        GameGrid baseGrid;
        GameGrid moveablesGrid;
        GameGrid placeablesGrid;
        switch (levelNumber)
        {
            case 0:
                return;
            default:
                return;
        }
    }

}
