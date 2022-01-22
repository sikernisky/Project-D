using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaster : MonoBehaviour
{
    /// <summary> The Player's saveable game data. </summary>
    private static PlayerData data = new PlayerData();

  
    /// <summary>Loads <c>path</c> into the current game data.
    /// <br></br><em>Precondition:</em> path is an existing JSON file.</summary>
    public void LoadGame(string path)
    {
        //IMPLEMENT ME! REMEMBER PRECONDITIONS
        throw new System.NotImplementedException();
    }

    /// <summary>Saves the current game data as a JSON file.</summary>
    public static void SaveGame()
    {
        //IMPLEMENT ME!
        throw new System.NotImplementedException();
    }

    public static List<Item> Inventory()
    {
        return data.nextInventory;
    }

    public static List<Item> UnlockedContainers()
    {
        return data.unlockedContainers;
    }


}
