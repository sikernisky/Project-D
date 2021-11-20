using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelData : MonoBehaviour
{
    public TMP_Text emText;

    public Image emImage;

    public static float levelEmBalance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        emText.text = levelEmBalance.ToString();
    }
}
