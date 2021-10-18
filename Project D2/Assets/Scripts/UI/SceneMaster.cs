using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    /**The Fader GameObject.*/
    public GameObject Fader;


    /**Instantly change scene to sceneName. If sceneName does not exist in build index,
     * does nothing. */
    public void ChangeScene(string sceneName)
    {
        if (!SceneExists(sceneName)) Debug.Log("Scene does not exist.");
        else SceneManager.LoadScene(sceneName);
    }

    /**Quits the game. */
    public void QuitGame()
    {
        Application.Quit();
    }

    /**Change scene to sceneName after one second. Triggers Fader
     * animation. If sceneName does not exist in build index, does nothing. */
    public void ChangeSceneWithFader(string sceneName)
    {
        if (!SceneExists(sceneName)) Debug.Log("Scene does not exist.");
        else if (Fader != null) StartCoroutine(ChangeSceneWithFaderCoro(sceneName));
        else ChangeScene(sceneName);
    }

    IEnumerator ChangeSceneWithFaderCoro(string sceneName)
    {

        Fader.GetComponent<Animator>().SetTrigger("triggerFader");
        yield return new WaitForSeconds(1);
        ChangeScene(sceneName);

    }


    /**Return true if scene exists, false otherwise.*/
    private bool SceneExists(string sceneName)
    {
        int numScenes = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[numScenes];
        for (int i = 0; i < numScenes; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }

        return Array.IndexOf(scenes, sceneName) > -1;
    }
}
