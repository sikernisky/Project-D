using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveMaster: MonoBehaviour
{
    private static string filePath1;
    private static string filePath2;
    private static string filePath3;


    public static SaveData saveData = new SaveData();

    private void Start()
    {
        filePath1 = Path.Combine(Application.persistentDataPath, "SaveData1.json");
        filePath2 = Path.Combine(Application.persistentDataPath, "SaveData2.json");
        filePath3 = Path.Combine(Application.persistentDataPath, "SaveData3.json");
    }

    /**Takes data from a save and loads it into the current SaveData.*/
    public static void LoadData(string loadSlot)
    {
        string fileName = "SaveData" + loadSlot + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) Debug.Log("File '" + fileName + "' is missing.");
        else
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, saveData);
        }
    }

    /**Takes the current SaveData and loads it into the current save slot.*/
    public void SaveData(string loadSlot)
    {
        WriteSaveFile(loadSlot);
    }

    /**Creates a new SaveData file and loads it into the current save slot.*/
    public static void NewGame()
    {
        if (OpenSlot() > 0) WriteSaveFile(OpenSlot().ToString());
        else Debug.Log("There are no save slots available; set up a system to delete one.");
    }

    /**Writes Json to SaveDataloadSlot.json.*/
    private static void WriteSaveFile(string loadSlot)
    {
        saveData.testName = "TestName is LoadSlot " + loadSlot; // DELETE
        string fileName = "SaveData" + loadSlot + ".json";
        string json = JsonUtility.ToJson(saveData);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, json);
    }

    /**Deletes the current save file.*/
    public void DeleteSave(string loadSlot)
    {
        string fileName = "SaveData" + loadSlot + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) Debug.Log("Can't delete file '" + fileName + "' because it doesn't exist.");
        else File.Delete(filePath);
    }


    /**Returns the earliest open load slot (1,2,3) or -1 if none are open.*/
    private static int OpenSlot()
    {
        if (!File.Exists(filePath1)) return 1;
        else if (!File.Exists(filePath2)) return 2;
        else if (!File.Exists(filePath3)) return 3;
        else return -1;
    }


}
