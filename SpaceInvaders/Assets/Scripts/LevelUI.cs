using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelUI : MonoBehaviour
{
    TMP_Text levelText;
    int prevLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        levelText = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentLevel = Global.currentLevel;
        if (prevLevel != currentLevel)
        {
            levelText.SetText("LEVEL = " + currentLevel);
            prevLevel = currentLevel;
        }
    }
}
